using System.Runtime.Serialization;

namespace Aec.Cqrs.Client.Projections
{
    [DataContract]
    public sealed class RegistrationView
    {
        [DataMember(Order = 1)]
        public string Status { get; set; }
        
        [DataMember(Order = 2)]
        public bool HasProblems { get; set; }
        
        [DataMember(Order = 3)]
        public bool Completed { get; set; }
        
        [DataMember(Order = 4)]
        public UserID UserID { get; set; }
        
        [DataMember(Order = 5)]
        public SecurityID SecurityID { get; set; }
        
        [DataMember(Order = 6)]
        public string UserDisplayName { get; set; }
        
        [DataMember(Order = 7)]
        public string UserToken { get; set; }
        
        [DataMember(Order = 8)]
        public string Problem { get; set; }
        
        [DataMember(Order = 9)]
        public string[] Permissions { get; set; }
        
        [DataMember(Order = 10)]
        public RegistrationID Registration { get; set; }

        public RegistrationView()
        {
            Permissions = new string[0];
        }
    }
}