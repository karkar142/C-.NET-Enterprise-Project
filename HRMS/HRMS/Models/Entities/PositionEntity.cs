using System.ComponentModel.DataAnnotations.Schema;

namespace HRMS.Models.Entities
{
    [Table("Postition")]
    public class PositionEntity : BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        private int level;

        public int Level
        {
            get { return level; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("Invilad level value");
                }
                level = value;
            }


    }   }
}
