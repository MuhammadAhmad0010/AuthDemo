using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthDemos.Core.Entities
{
    public class TypeFormAnswers
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Submission))]
        public int SubmissionId { get; set; }
        public TypeFormSubmission Submission { get; set; }

        public string FieldRef { get; set; }
        public string QuestionTitle { get; set; }
        public string FieldType { get; set; }
        public string AnswerText { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
