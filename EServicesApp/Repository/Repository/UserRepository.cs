using System;
using System.Collections.Generic;
using System.Linq;
using Data.Config;
using Data.Models;
using Repository.DTOModel;
using System.Web;
//using System.Web.Hosting;

namespace Repository.Repository
{
    public class UserRepository
    {
        public FullUserDto ValidateUser(string email, string password)
        {
            using (var dbContext = new OmContext())
            {
                var user = dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
                if (user != null)
                {
                    var userWithRoles = new FullUserDto
                    {
                        UserId = user.UserId,
                        Name = user.Name,
                        Email = user.Email,
                        UserRoles = (from ur in dbContext.UserRoles
                            join r in dbContext.Roles on ur.RoleId equals r.RoleId
                            where ur.UserId == user.UserId
                            select new RoleDto {Title = r.Title}).ToList(),
                        UserViews = (from uv in dbContext.UserViews
                                     join v in dbContext.Views on uv.ViewId equals v.ViewId
                                     where uv.UserId == user.UserId
                                     select new ViewDto { Title =v.Title}).ToList()
                    };
                    return userWithRoles;
                }

                return null;
            }
        }

        public int AddUser(string name, string email, string password, string cnic)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var user = dbContext.Users.FirstOrDefault(a => a.Email == email);
                    if (user == null)
                    {
                        User newUser = new User()
                        {
                            Name = name,
                            Email = email,
                            Password = password,
                            cnic = cnic
                        };

                        dbContext.Users.Add(newUser);
                        dbContext.SaveChanges();

                        return newUser.UserId;
                    }
                    return 0;
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        //Update Password
        public int UpdatePassword(String prePassword, String newPassword, int userId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.UserId == userId && u.Password == prePassword);

                    if (user.UserId.Equals(userId))
                    {
                        user.Password = newPassword;
                        dbContext.SaveChanges();
                        return 1;
                    }

                    return 0;
                }
                catch (Exception ex)
                {
                    return -1;
                }
            }

        }
        public bool AddNotificationToken(int userId, string token)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var userNotification = dbContext.NotificationDetails.FirstOrDefault(u => u.UserId == userId);
                    if (userNotification == null)
                    {
                        dbContext.NotificationDetails.Add(new NotificationDetail
                        {
                            UserId = userId,
                            Token = token
                        });
                    }else
                    {
                        userNotification.Token = token;
                    }
                    dbContext.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }

            //Upload image
        public int UploadImage(HttpContext context, int userId)
        {
            using (var dbContext = new OmContext())
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                try
                {
                    var httpRequest = context.Request;//HttpContext.Current.Request;

                    foreach (string file in httpRequest.Files)
                    {
                        var postedFile = httpRequest.Files[file];
                        if (postedFile != null && postedFile.ContentLength > 0)
                        {

                            int MaxContentLength = 1024 * 1024 * 1; //Size = 1 MB  

                            IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                            var ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                            var extension = ext.ToLower();
                            if (!AllowedFileExtensions.Contains(extension))
                            {

                                var message = string.Format("Please Upload image of type .jpg,.gif,.png.");

                                dict.Add("error", message);
                                return -1;
                            }
                            /*else if (postedFile.ContentLength > MaxContentLength)
                            {

                                var message = string.Format("Please Upload a file upto 1 mb.");

                                dict.Add("error", message);
                                return Request.CreateResponse(HttpStatusCode.BadRequest, dict);
                            }*/
                            else
                            {
                                var filePath = HttpContext.Current.Server.MapPath("~/Userimage/" + userId + extension);

                                postedFile.SaveAs(filePath);

                                ProfileImage newImage = new ProfileImage
                                {
                                    UserId = userId,
                                    ImageUrl = filePath
                                };
                                dbContext.ProfileImages.Add(newImage);
                                dbContext.SaveChanges();

                            }
                        }

                        var message1 = string.Format("Image Updated Successfully.");
                        return 1;
                    }
                    var res = string.Format("Please Upload a image.");
                    dict.Add("error", res);
                    return -1;
                }
                catch (Exception ex)
                {
                    var res = ex.ToString();// string.Format(ex);
                    dict.Add("error", res);
                    return -1;
                }
            }
        }

        public string GetImage(int userId)
        {
            using (var dbContext = new OmContext())
            {
                var image = dbContext.ProfileImages.FirstOrDefault(u => u.UserId == userId);
                if (image == null)
                {
                    return null;
                }
                return image.ImageUrl;

            }
        }
    }
}