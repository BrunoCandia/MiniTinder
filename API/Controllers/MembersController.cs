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

        public MembersController(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
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
        public async Task<ActionResult<IReadOnlyList<Photo>>> GetPhotosForMember(Guid id)
        {
            var photos = await _memberRepository.GetPhotosForMemberAsync(id);

            if (photos is null || photos.Count == 0)
            {
                return NotFound();
            }

            return Ok(photos);
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
    }
}
