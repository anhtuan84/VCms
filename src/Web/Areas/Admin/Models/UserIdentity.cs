using Microsoft.Practices.Unity;
using Models.Service;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace Web.Areas.Admin.Models
{
    public class UserIdentity : IIdentity
    {
        private string userName = string.Empty;
        private string fullName = string.Empty;
        private ArrayList roles = new ArrayList();
        private bool authenticated = false;
        #region IIdentity Members
        public string AuthenticationType
        {
            get { return "ADMIN_SECURITY"; }
        }
        public bool IsAuthenticated
        {
            get { return authenticated; }
        }
        public string Name
        {
            get { return userName; }
        }
        public string FullName
        {
            get { return fullName; }
        }
        internal bool IsInRole(string role)
        {
            return roles.Contains(role);
        }
        #endregion


       
        #region Constructor(s)
        public UserIdentity(dynamic usrDetails)
        {            
            var user = usrDetails;
            this.userName = user.UserName;
            this.fullName = user.UserName + "[" + user.IpAddress + "]";
            this.roles.Clear();
            authenticated = true;                   
            roles.AddRange(user.Permissions);
        }       
        #endregion
    }
}