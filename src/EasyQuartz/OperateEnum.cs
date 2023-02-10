using System.ComponentModel;

namespace EasyQuartz
{
    public enum OperateEnum
    {
        [Description("删除")]
        Delete = 0,
        [Description("暂停")]
        Pause = 1,
        [Description("恢复")]
        Resume = 2
    }
}