using System;

namespace NetCoreReactTempl.Web.API.Dto
{
    public class Data : BaseDto
    {
        public long UserId { get; set; }
        public string Field1 { get; set; }
        public DateTime Field2 { get; set; }
        public bool Field3 { get; set; }
        public RadioButton Field4 { get; set; }
        public DropDown Field5 { get; set; }
    }

    public enum RadioButton : int {
        radio1,
        radio2
    }

    public enum DropDown : int {
        option1,
        option2,
        option3
    }
}
