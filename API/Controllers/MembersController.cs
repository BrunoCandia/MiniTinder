using API.Data;
using API.DTO;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    //[Route("api/[controller]")] // https://localhost:5001/api/members
    //[ApiController]
    [Authorize]
    public class MembersController : BaseApiController
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IPhotoService _photoService;

        public MembersController(IMemberRepository memberRepository, IPhotoService photoService)
        {
            _memberRepository = memberRepository;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<MemberDto>>> GetMembers()
        {
            var users = await _memberRepository.GetMembersAsync();

            return Ok(users);
        }

        [HttpGet("{id}")] // https://localhost:5001/api/members/A0E8162D-152A-F111-87A6-E8039A9A54C4
        public async Task<ActionResult<User>> GetMember(Guid id)
        {
            var user = await _memberRepository.GetMemberByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpGet("{id}/photos")] // https://localhost:5001/api/members/A0E8162D-152A-F111-87A6-E8039A9A54C4/photos
        public async Task<ActionResult<IReadOnlyList<PhotoResponseDto>>> GetPhotosForMember(Guid id)
        {
            var photosDto = await _memberRepository.GetPhotosForMemberAsync(id);

            if (photosDto is null || photosDto.Count == 0)
            {
                return NotFound();
            }

            var photoResponseDto = photosDto.Select(p => new PhotoResponseDto
            {
                Id = p.Id,
                Url = p.Url,
                FileName = p.PublicId ?? string.Empty,
                MemberId = p.MemberId
            }).ToList();

            return Ok(photoResponseDto);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateMember(MemberUpdateDto memberUpdateDto)
        {
            var memberId = User.GetMemberId();

            if (string.IsNullOrWhiteSpace(memberId))
            {
                return BadRequest("Invalid Member ID");
            }

            var member = await _memberRepository.GetMemberForUpdate(Guid.Parse(memberId));
            //var memberDto = await _memberRepository.GetMemberByIdAsync(Guid.Parse(memberId));

            if (member is null)
            {
                return BadRequest("Could not get member");
            }

            // Update member properties
            member.DisplayName = memberUpdateDto.DisplayName ?? member.DisplayName;
            member.Description = memberUpdateDto.Description ?? member.Description;
            member.City = memberUpdateDto.City ?? member.City;
            member.Country = memberUpdateDto.Country ?? member.Country;

            member.User.DisplayName = memberUpdateDto.DisplayName ?? member.User.DisplayName;

            _memberRepository.Update(member);

            if (await _memberRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to update member");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoResponseDto>> AddPhoto([FromForm] IFormFile file)
        {
            var memberId = User.GetMemberId();

            if (string.IsNullOrWhiteSpace(memberId))
            {
                return BadRequest("Invalid Member ID");
            }

            var member = await _memberRepository.GetMemberForUpdate(Guid.Parse(memberId));

            if (member is null)
            {
                return BadRequest("Could not get member");
            }

            var photoResponseDto = await _photoService.UploadPhotoAsync(file);

            if (photoResponseDto is null || string.IsNullOrWhiteSpace(photoResponseDto.Url) || string.IsNullOrWhiteSpace(photoResponseDto.FileName))
            {
                return BadRequest("Failed to upload photo");
            }

            var photo = new Photo
            {
                Url = photoResponseDto.Url,
                PublicId = photoResponseDto.FileName,
                MemberId = Guid.Parse(memberId)
            };

            if (member.ImageUrl is null)
            {
                member.ImageUrl = photoResponseDto.Url;
                member.User.ImageUrl = photoResponseDto.Url;
            }

            member.Photos.Add(photo);

            if (await _memberRepository.SaveAllAsync())
            {
                photoResponseDto.Id = photo.Id;
                photoResponseDto.MemberId = photo.MemberId;

                return Ok(photoResponseDto);
            }

            return BadRequest("Failed to add photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var memberId = User.GetMemberId();

            if (string.IsNullOrWhiteSpace(memberId))
            {
                return BadRequest("Invalid Member ID");
            }

            var member = await _memberRepository.GetMemberForUpdate(Guid.Parse(memberId));

            if (member is null)
            {
                return BadRequest("Could not get member");
            }

            var photo = member.Photos.FirstOrDefault(p => p.Id == photoId);

            if (photo is null || member.ImageUrl == photo.Url)
            {
                return BadRequest("Could not set this photo as the main photo");
            }

            member.ImageUrl = photo.Url;
            member.User.ImageUrl = photo.Url;

            if (await _memberRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var memberId = User.GetMemberId();

            if (string.IsNullOrWhiteSpace(memberId))
            {
                return BadRequest("Invalid Member ID");
            }

            var member = await _memberRepository.GetMemberForUpdate(Guid.Parse(memberId));
            if (member is null)
            {
                return BadRequest("Could not get member");
            }

            var photo = member.Photos.FirstOrDefault(p => p.Id == photoId);
            
            if (photo is null)
            {
                return NotFound();
            }

            if (member.ImageUrl == photo.Url)
            {
                return BadRequest("You cannot delete your main photo");
            }

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (!result)
                {
                    return BadRequest("Failed to delete photo from cloud storage");
                }
            }
                                    
            member.Photos.Remove(photo);
            
            if (await _memberRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to delete photo");
        }
    }
}
