using Models.Enums;

namespace Logic.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public CategoryType TypeOf { get; set; }
        public StatusType Status { get; set; }
        public CategoryDto() { }

        public CategoryDto(int id, string categoryName, DateTime categoryCreationDate, CategoryType categoryType, StatusType statusType)
        {
            Id = id;
            Name = categoryName;
            CreationDate = categoryCreationDate;
            TypeOf = categoryType;
            Status = statusType;
        }
    }
} 