using API.DTO;
using API.Entities;

namespace API.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member?> GetMemberForUpdate(Guid id);
        void Update(Member member);
        Task<bool> SaveAllAsync();
        Task<IReadOnlyList<MemberDto>> GetMembersAsync();
        Task<MemberDto?> GetMemberByIdAsync(Guid id);
        Task<IReadOnlyList<PhotoDto>> GetPhotosForMemberAsync(Guid memberId);
    }
}
