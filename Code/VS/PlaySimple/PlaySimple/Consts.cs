namespace PlaySimple
{
    public static class Consts
    {
        public const string DB_PATH = "~/App_Data/db.sqlite";

        public static class Roles
        {
            public const string Customer = "Customer";
            public const string Employee = "Employee";
            public const string Admin = "Admin";
            public const string None = "None";
        }

        public static class Paging
        {
            public const int PageSize = 10;
        }

        public static class Decodes
        {
            public enum OrderStatus
            {
                Sent=1,
                Accepted,
                Rejected,
                Canceled
            }

            public enum ComplaintType
            {
                Payment=1,
                Attendance,
                Sportsmanship
            }

            public enum FieldSize
            {
                Small = 1,
                Medium,
                Large
            }

            public enum FieldType
            {
                Football=1,
                Basketball,
                Tennis,
                Volleyball
            }

            public enum InvitationStatus
            {
                Sent=1,
                Accepted,
                Rejected
            }

            public enum RegionDecode
            {
                Dan=1,
                Negev,
                Haifa,
                Jerusalem
            }
        }
    }
}