namespace NetCoreAPI.Models
{
    public class School
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Street { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public long[] Location { get; set; }

        public int Fees { get; set; }

        public string[] Tags { get; set; }

        public string Rating { get; set; }
    }
}