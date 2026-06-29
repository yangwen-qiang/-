using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 智能罐装生产线监控系统.Models
{
    [Table(Name = "SystemLog", DisableSyncStructure = true)]
    public class SystemLog
    {
        [Column(Name="Id",IsPrimary =true,IsIdentity =true)]
        public int Id { get; set; }
        [Column(Name = "Timestamp")]
        public DateTime Timestamp { get; set; }
        [Column(Name = "Level",StringLength =50)]
        public string Level { get; set; }
        [Column(Name = "Exception",StringLength =1000)]
        public int Exception { get; set; }
        [Column(Name = "RenderedMessage",StringLength =50)]
        public string RenderedMessage { get; set; }
        [Column(Name = "Properties", StringLength = 1000)]
        public string Properties { get; set; }


    }
}
