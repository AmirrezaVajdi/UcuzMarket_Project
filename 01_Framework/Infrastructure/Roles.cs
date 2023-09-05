namespace _01_Framework.Infrastructure
{
    public class Roles
    {
        public const string Administrator = "1";
        public const string SystemUser = "2";
        public const string ContentUploader = "3";
        public const string ColleagueUser = "4";


        public static string GetRoleBy(long id)
        {
            var result = id switch
            {
                1 => "مدیر سیستم",
                3 => "محتوا گذار",
                _ => ""
            };
            return result;
        }
    }
}
