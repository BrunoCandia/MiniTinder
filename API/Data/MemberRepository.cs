using API.DTO;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class MemberRepository : IMemberRepository
    {
        private readonly DataContext _context;

        public MemberRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<MemberDto?> GetMemberByIdAsync(Guid id)
        {
            var member = await _context.Members.FindAsync(id);

            if (member is null)
            {
                return null;
            }

            var memberDto = new MemberDto
            {
                Id = member.Id,
                DateOfBirth = member.DateOfBirth,
                ImageUrl = member.ImageUrl,
                DisplayName = member.DisplayName,
                Created = member.Created,
                LastActive = member.LastActive,
                Gender = member.Gender,
                Description = member.Description,
                City = member.City,
                Country = member.Country
            };

            return memberDto;
        }

        public async Task<IReadOnlyList<MemberDto>> GetMembersAsync()
        {
            var members = await _context.Members.ToListAsync();

            var membersDto = members.Select(m => new MemberDto
            {
                Id = m.Id,
                DateOfBirth = m.DateOfBirth,
                ImageUrl = m.ImageUrl,
                DisplayName = m.DisplayName,
                Created = m.Created,
                LastActive = m.LastActive,
                Gender = m.Gender,
                Description = m.Description,
                City = m.City,
                Country = m.Country
            }).ToList();


            return membersDto;
        }

        public async Task<IReadOnlyList<PhotoDto>> GetPhotosForMemberAsync(Guid memberId)
        {
            var photos = await _context.Members
                .Where(m => m.Id == memberId)
                .SelectMany(m => m.Photos)
                .ToListAsync();

            var photosDto = photos.Select(p => new PhotoDto
            {
                Id = p.Id,
                Url = p.Url,
                PublicId = p.PublicId,
                IsApproved = p.IsApproved,
                MemberId = p.MemberId
            }).ToList();

            return photosDto;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Member member)
        {
            _context.Entry(member).State = EntityState.Modified;
        }

        public async Task<Member?> GetMemberForUpdate(Guid id)
        {
            return await _context.Members
                .Include(x => x.User)
                .Include(x => x.Photos)
                .IgnoreQueryFilters()
                .SingleOrDefaultAsync(x => x.Id == id);
        }
    }
}
