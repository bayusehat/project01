using project01.Models.DB;
using project01.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;

namespace project01.Models.EntityManager
{
    public class UserManager
    {
        public void AddUserAccount(UserSignUpView user)
        {
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                SYSUser SU = new SYSUser();
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowMOdifiedDateTime = DateTime.Now;
                db.SYSUsers.Add(SU);
                db.SaveChanges();
                SYSUserProfile SUP = new SYSUserProfile();
                SUP.SYSUserID = SU.SYSUserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.Gender = user.Gender;
                SUP.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;
                db.SYSUserProfiles.Add(SUP);
                db.SaveChanges();
                if (user.LOOKUPRoleID > 0)
                {
                    SYSUserRole SUR = new SYSUserRole();
                    SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                    SUR.SYSUserID = user.SYSUserID;
                    SUR.IsActive = true;
                    SUR.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID :
                    1;
                    SUR.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID :
                    1;
                    SUR.RowCreatedDateTime = DateTime.Now;
                    SUR.RowModifiedDateTime = DateTime.Now;
                    db.SYSUserRoles.Add(SUR);
                    db.SaveChanges();
                }
            }
        }
        public void AddPelanggan(Pelanggan user)
        {
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                pelanggan PU = new pelanggan();
                PU.no_id = user.no_id;
                PU.nama = user.nama;
                PU.alamat = user.alamat;
                PU.no_tlp = user.no_tlp;
                PU.no_tlp2 = user.no_tlp2;
                db.pelanggans.Add(PU);
                db.SaveChanges();
            }
        }
        public void AddOutbond(Outbond user)
        {
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                outbond OU = new outbond();
                OU.keterangan = user.keterangan;
                OU.harga = user.harga;
                db.outbonds.Add(OU);
                db.SaveChanges();
            }
        }
        public bool IsLoginNameExist(string loginName)
        {
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                return db.SYSUsers.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }
        public string GetUserPassword(string loginName)
        {
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                var user = db.SYSUsers.Where(o =>
                o.LoginName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().PasswordEncryptedText;
                else
                    return string.Empty;
            }
        }
        public bool IsUserInRole(string loginName, string roleName)
        {
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                SYSUser SU = db.SYSUsers.Where(o =>
                o.LoginName.ToLower().Equals(loginName))?.FirstOrDefault();
                if (SU != null)
                {
                    var roles = from q in db.SYSUserRoles
                                join r in db.LOOKUPRoles on q.LOOKUPRoleID equals
                                r.LOOKUPRoleID
                                where r.RoleName.Equals(roleName) &&
                                q.SYSUserID.Equals(SU.SYSUserID)
                                select r.RoleName;
                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }
                return false;
            }
        }
        public List<LOOKUPAvailableRole> GetAllRoles()
        {
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                var roles = db.LOOKUPRoles.Select(o => new LOOKUPAvailableRole
                {
                    LOOKUPRoleID = o.LOOKUPRoleID,
                    RoleName = o.RoleName,
                    RoleDescription = o.RoleDescription
                }).ToList();
                return roles;
            }
        }
        public int GetUserID(string loginName)
        {
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                var user = db.SYSUsers.Where(o => o.LoginName.Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().SYSUserID;
            }
            return 0;
        }
        public List<UserProfileView> GetAllUserProfiles()
        {
            List<UserProfileView> profiles = new List<UserProfileView>();
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                UserProfileView UPV;
                var users = db.SYSUsers.ToList();
                foreach (SYSUser u in db.SYSUsers)
                {
                    UPV = new UserProfileView(); UPV.SYSUserID = u.SYSUserID;
                    UPV.LoginName = u.LoginName;
                    UPV.Password = u.PasswordEncryptedText;
                    var SUP = db.SYSUserProfiles.Find(u.SYSUserID);
                    if (SUP != null)
                    {
                        UPV.FirstName = SUP.FirstName;
                        UPV.LastName = SUP.LastName;
                        UPV.Gender = SUP.Gender;
                    }
                    var SUR = db.SYSUserRoles.Where(o => o.SYSUserID.Equals(u.SYSUserID));
                    if (SUR.Any())
                    {
                        var userRole = SUR.FirstOrDefault();
                        UPV.LOOKUPRoleID = userRole.LOOKUPRoleID;
                        UPV.RoleName = userRole.LOOKUPRole.RoleName;
                        UPV.IsRoleActive = userRole.IsActive;
                    }
                    profiles.Add(UPV);
                }
            }
            return profiles;
        }
        public UserDataView GetUserDataView(string loginName)
        {
            UserDataView UDV = new UserDataView();
            List<UserProfileView> profiles = GetAllUserProfiles();
            List<LOOKUPAvailableRole> roles = GetAllRoles();
            int? userAssignedRoleID = 0, userID = 0;
            string userGender = string.Empty;
            userID = GetUserID(loginName);
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                userAssignedRoleID = db.SYSUserRoles.Where(o => o.SYSUserID == userID)?.FirstOrDefault().LOOKUPRoleID;
                userGender = db.SYSUserProfiles.Where(o => o.SYSUserID ==
                userID)?.FirstOrDefault().Gender;
            }
            List<Gender> genders = new List<Gender>();
            genders.Add(new Gender { Text = "Male", Value = "M" });
            genders.Add(new Gender { Text = "Female", Value = "F" });
            UDV.UserProfile = profiles;
            UDV.UserRoles = new UserRoles
            {
                SelectedRoleID = userAssignedRoleID,
                UserRoleList = roles
            };
            UDV.UserGender = new UserGender
            {
                SelectedGender = userGender,
                Gender = genders
            };
            return UDV;
        }
        public List<Pelanggan> GetAllPelanggan()
        {
            List<Pelanggan> profiles = new List<Pelanggan>();
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                Pelanggan UPV;
                var users = db.pelanggans.ToList();
                foreach (pelanggan u in db.pelanggans)
                {
                    UPV = new Pelanggan();
                    UPV.no_id = u.no_id;
                    UPV.nama = u.nama;
                    UPV.alamat = u.alamat;
                    UPV.no_tlp = u.no_tlp;
                    UPV.no_tlp2 = u.no_tlp2;

                    profiles.Add(UPV);
                }
            }
            return profiles;
        }
        public Pelanggan GetAllPelanggan(string loginName)
        {
            Pelanggan GAP = new Pelanggan();
            List<Pelanggan> profiles = GetAllPelanggan();
            List<LOOKUPAvailableRole> roles = GetAllRoles();
            int? userID = 0;
            string userGender = string.Empty;
            userID = GetUserID(loginName);


            GAP.UserProfile = profiles;
            return GAP;
        }
        public List<Outbond> GetAllOutbond()
        {
            List<Outbond> profiles = new List<Outbond>();
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                Outbond UPV;
                var users = db.outbonds.ToList();
                foreach (outbond u in db.outbonds)
                {
                    UPV = new Outbond();
                    UPV.keterangan = u.keterangan;
                    UPV.harga = u.harga;


                    profiles.Add(UPV);
                }
            }
            return profiles;
        }
        public Outbond GetAllOutbond(string loginName)
        {
            Outbond GAO = new Outbond();
            List<Outbond> profiles = GetAllOutbond();
            List<LOOKUPAvailableRole> roles = GetAllRoles();
            int? userID = 0;
            string userGender = string.Empty;
            userID = GetUserID(loginName);


            GAO.UserProfile = profiles;
            return GAO;
        }

        public void UpdateUserAccount(UserProfileView user)
        {
            using (DBOutbondEntities1 db = new DBOutbondEntities1())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        SYSUser SU = db.SYSUsers.Find(user.SYSUserID);
                        SU.LoginName = user.LoginName;
                        SU.PasswordEncryptedText = user.Password;
                        SU.RowCreatedSYSUserID = user.SYSUserID;
                        SU.RowModifiedSYSUserID = user.SYSUserID;
                        SU.RowCreatedDateTime = DateTime.Now;
                        SU.RowMOdifiedDateTime = DateTime.Now;
                        db.SaveChanges();
                        var userProfile = db.SYSUserProfiles.Where(o => o.SYSUserID ==
                        user.SYSUserID);
                        if (userProfile.Any())
                        {
                            SYSUserProfile SUP = userProfile.FirstOrDefault();
                            SUP.SYSUserID = SU.SYSUserID; SUP.FirstName = user.FirstName;
                            SUP.LastName = user.LastName;
                            SUP.Gender = user.Gender;
                            SUP.RowCreatedSYSUserID = user.SYSUserID;
                            SUP.RowModifiedSYSUserID = user.SYSUserID;
                            SUP.RowCreatedDateTime = DateTime.Now;
                            SUP.RowModifiedDateTime = DateTime.Now;
                            db.SaveChanges();
                        }
                        if (user.LOOKUPRoleID > 0)
                        {
                            var userRole = db.SYSUserRoles.Where(o => o.SYSUserID ==
                            user.SYSUserID);
                            SYSUserRole SUR = null;
                            if (userRole.Any())
                            {
                                SUR = userRole.FirstOrDefault();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                            }
                            else
                            {
                                SUR = new SYSUserRole();
                                SUR.LOOKUPRoleID = user.LOOKUPRoleID;
                                SUR.SYSUserID = user.SYSUserID;
                                SUR.IsActive = true;
                                SUR.RowCreatedSYSUserID = user.SYSUserID;
                                SUR.RowModifiedSYSUserID = user.SYSUserID;
                                SUR.RowCreatedDateTime = DateTime.Now;
                                SUR.RowModifiedDateTime = DateTime.Now;
                                db.SYSUserRoles.Add(SUR);
                            }
                            db.SaveChanges();
                        }
                        dbContextTransaction.Commit();
                    }
                    catch
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }
    }
}