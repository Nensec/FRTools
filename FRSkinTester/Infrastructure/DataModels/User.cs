using Microsoft.AspNet.Identity;

namespace FRSkinTester.Infrastructure.DataModels
{
    public class User : IUser<int>
    {
        public int Id { get; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}