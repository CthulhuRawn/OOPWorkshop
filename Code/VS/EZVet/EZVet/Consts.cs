namespace EZVet
{
    public static class Consts
    {
        public const string DB_PATH = "~/App_Data/db.sqlite";

        public static class Roles
        {
            public const string Owner = "Owner";
            public const string Doctor = "Doctor";
            public const string None = "None";
        }

        public static class Decodes
        {
            public enum AnimalType
            {
                Cat=1,
                Dog,
                Fish,
                Bird
            }

            public enum Gender
            {
                Male=1,
                Female
            }

            public enum TreatmentType
            {
                Treatment = 1,
                Medicine,
                Vaccine
            }
       }
    }
}