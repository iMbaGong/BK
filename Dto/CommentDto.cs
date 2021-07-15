using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class CommentDto
    {
        public CommentDto(Comment comment)
        {
            create_time = comment.create_time;
            grade = comment.grade;
            detail = comment.detail;
            comment_status = comment.comment_status;
        }

        public DateTime create_time { get; set; }

        public decimal? grade { get; set; }

        public string detail { get; set; }

        public decimal comment_status { get; set; }

    }
}