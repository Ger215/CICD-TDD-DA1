using Models;

namespace Logic.DTOs
{
    public class ReportGoalByCategoryDto
    {
        public double TotalSpent { get; set; }
        public int PercentageSpent { get; set; }
        public Category CategoryAssociated { get; set; }
        
        public ReportGoalByCategoryDto() { }

        public ReportGoalByCategoryDto(Category categoryAssociated, double totalSpended, int percentageSpent)
        {
            CategoryAssociated = categoryAssociated;
            PercentageSpent = percentageSpent;
            TotalSpent = totalSpended;
        }
    }
} 