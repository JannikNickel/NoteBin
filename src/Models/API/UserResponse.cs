namespace NoteBin.Models.API
{
    public record UserResponse(string Username, long CreationTime)
    {
        internal static UserResponse FromUser(User user) => new UserResponse(user.Name, TimeUtils.ToUnixTimeMilliseconds(user.CreationTime));
    }
}