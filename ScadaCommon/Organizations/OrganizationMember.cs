namespace ScadaCommon
{
    public class OrganizationMember
    {
        public int ID{get;set;}
        public string UserName {get;set;}
        public bool isAdmin {get;set;}

        public int OrganizationId{get;set;}
        public Organization Organization {get;set;}
    }
}