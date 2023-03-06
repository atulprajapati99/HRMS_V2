using System.ComponentModel;
using System.Reflection;

namespace HRMS_V2.Infrastructure.Securiy
{
    internal class PermissionDetails : Attribute
    {
        public PermissionDetails(Group group, Permission permission, int permissionOrder, Feature feature)
        {
            GroupName = GetEnumDescription(group);
            GroupOrder = (int)group;
            PermissionName = GetEnumDescription(permission);
            PermissionOrder = permissionOrder;
            Feature = feature;
        }

        public string GroupName { get; }

        public int PermissionOrder { get; }

        public string PermissionName { get; }

        public int GroupOrder { get; }

        public Feature Feature { get; }

        private string GetEnumDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }
    }
}
