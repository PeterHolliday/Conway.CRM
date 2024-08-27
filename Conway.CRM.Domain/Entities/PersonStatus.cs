namespace Conway.CRM.Domain.Entities
{
    public class PersonStatus
    {
        public Guid Id { get; set; }

        public string StatusCode { get; set; }

        public string StatusText { get; set; }

        public int SortOrder { get; set; }

        public List<Person>? People { get; set; }
    }
}
