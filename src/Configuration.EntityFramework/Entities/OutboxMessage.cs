namespace Configuration.EntityFramework.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; private set; }
        public DateTime? DateTimeOccurred { get; private set; }
        public string Type { get; private set; }
        public string Content { get; private set; }
        public string? Error { get; private set; }

        public OutboxMessage(Guid id, DateTime? dateTimeOccurred, string type, string content)
        {
            Id = id;
            DateTimeOccurred = dateTimeOccurred;
            Type = type;
            Content = content;
        }
    }
}
