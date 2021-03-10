using System;
using System.Collections.Generic;
using System.Linq;
using Data.Config;
using Data.Models;
using Repository.DTOModel;
using Repository.Repository;
using AutoMapper;

namespace HeadsUp.Repository.Repositories
{
    public class CustomerRepository
    {
        private readonly UserRepository _userRepository;

        public CustomerRepository()
        {
            _userRepository = new UserRepository();
        }
        public SimpleUserDto AddFranchiseCustomer(SimpleUserDto franchiseCustomerDto,int userId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {

                    var franchise = (from f in dbContext.Franchises
                                     join fa in dbContext.FranchiseAdmins on f.FranchiseId equals fa.FranchiseId
                                     where fa.UserId == userId
                                     select f).FirstOrDefault();
                    if (franchise != null)
                    {
                        var user = dbContext.Users.FirstOrDefault(a => a.Email == franchiseCustomerDto.Email);
                        if (user == null)
                        {
                            
                            User newUser = new User()
                            {
                                Name = franchiseCustomerDto.Name,
                                Email = franchiseCustomerDto.Email,
                                Password = franchiseCustomerDto.Password,
                                cnic = franchiseCustomerDto.cnic,
                                phone=franchiseCustomerDto.phone
                            };
                            dbContext.Users.Add(newUser);
                            var role = dbContext.Roles.FirstOrDefault(r => r.Title == "Customer");
                            if (role != null)
                            {
                                var userRole = new UserRole
                                {
                                    UserId = newUser.UserId,
                                    RoleId = role.RoleId
                                };
                                dbContext.UserRoles.Add(userRole);
                                dbContext.SaveChanges();

                                Address newAddress = new Address()
                                {
                                    Country = "Pakistan",//franchiseCustomerDto.CustomerAddres.Country,
                                    House = "1",//franchiseCustomerDto.CustomerAddres.House,
                                    State = "Islamabad",//franchiseCustomerDto.CustomerAddres.State,
                                    Street = "H-10"//franchiseCustomerDto.CustomerAddres.Street
                                };
                                dbContext.Addresses.Add(newAddress);
                                dbContext.SaveChanges();

                                Customer newCustomer = new Customer()
                                {
                                    FranchiseId = franchise.FranchiseId,
                                    UserId = newUser.UserId,
                                    AddressId = newAddress.AddressId
                                };
                                dbContext.Customers.Add(newCustomer);
                                dbContext.SaveChanges();

                                return Mapper.Map<SimpleUserDto>(newUser);
                            }
                        }
                    }
                    return null;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public List<SimpleUserDto> GetAllFranchisesCustomers(int userId)
        {
            using (var dbContext = new OmContext())
            {
                var franchisesCustomers = (from fw in dbContext.Customers
                                         join f in dbContext.Franchises on fw.FranchiseId equals f.FranchiseId
                                         join fa in dbContext.FranchiseAdmins on f.FranchiseId equals fa.FranchiseId
                                         join u in dbContext.Users on fw.UserId equals u.UserId
                                         where fa.UserId == userId
                                         select new SimpleUserDto
                                         {
                                             UserId = u.UserId,
                                             Name = u.Name,
                                             Email = u.Email,
                                             Password = u.Password,
                                             cnic = u.cnic,
                                             phone=u.phone
                                         }).ToList();
                return franchisesCustomers;
            }
        }

        public FranchiseCustomerDto GetFranchiseCustomer(int userId)
        {
            using (var dbContext = new OmContext())
            {
                var franchises = (from u in dbContext.Users
                                  join c in dbContext.Customers on u.UserId equals c.UserId
                                  join a in dbContext.Addresses on c.AddressId equals a.AddressId
                                  where u.UserId ==userId
                                  select new FranchiseCustomerDto
                                  {
                                      FranchiseId = c.FranchiseId,
                                      CustomerId=c.CustomerId,
                                      FranchiseCustomer = new SimpleUserDto
                                      {
                                          UserId = u.UserId,
                                          Name = u.Name,
                                          Email = u.Email,
                                          Password = u.Password,
                                          cnic = u.cnic,
                                          phone=u.phone
                                      },
                                      CustomerAddres = new AddressDto
                                      {
                                          AddressId=a.AddressId,
                                          Country=a.Country,
                                          State=a.State,
                                          Street=a.Street,
                                          House=a.House,
                                          Latitude=a.Latitude,
                                          Longitude=a.Longitude
                                      }
                                  }).FirstOrDefault();
                return franchises;
            }
        }
        public bool DeleteFranchiseCustomer(int userId)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var user = dbContext.Users.FirstOrDefault(u => u.UserId == userId);
                    if (user != null)
                    {
                        var worker = dbContext.Customers.FirstOrDefault(u => u.UserId == user.UserId);
                        if (worker != null)
                        {
                            var roles = dbContext.UserRoles.FirstOrDefault(a => a.UserId == user.UserId);
                            dbContext.UserRoles.Remove(roles);
                            dbContext.Customers.Remove(worker);
                            dbContext.Users.Remove(user);
                            dbContext.SaveChanges();
                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
        public SimpleUserDto UpdateFranchiseCustomer(SimpleUserDto franchiseCustomerDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var user = dbContext.Users.FirstOrDefault(a => a.Email == franchiseCustomerDto.Email);
                    if (user != null)
                    {
                        user.Name = franchiseCustomerDto.Name;
                        user.Email = franchiseCustomerDto.Email;
                        user.Password = franchiseCustomerDto.Password;
                        user.cnic = franchiseCustomerDto.cnic;
                        user.phone = franchiseCustomerDto.phone;
                    }
                    if (user == null)
                    {
                        User newUser = new User()
                        {
                            Name = franchiseCustomerDto.Name,
                            Email = franchiseCustomerDto.Email,
                            Password = franchiseCustomerDto.Password,
                            cnic = franchiseCustomerDto.cnic,
                            phone=franchiseCustomerDto.phone
                        };

                    }
                    dbContext.SaveChanges();

                    return Mapper.Map<SimpleUserDto>(user);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        //Update House Owner Location
        public CustomerLocation UpdateLocation(CustomerLocation customerLocation)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var customer = dbContext.Customers.FirstOrDefault(h => h.UserId == customerLocation.UserId);

                    var address = dbContext.Addresses.First(a => a.AddressId == customer.AddressId);
                    address.Latitude = customerLocation.Latitude;
                    address.Longitude = customerLocation.Longitude;

                    dbContext.SaveChanges();

                    return new CustomerLocation() {
                        UserId=customer.UserId,
                        Latitude=address.Latitude,
                        Longitude=address.Longitude
                    }
                        ;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
        }

        //Update Address
        public bool UpdateAddress(AddressDto addressDto)
        {
            using (var dbContext = new OmContext())
            {
                try
                {
                    var address = dbContext.Addresses.FirstOrDefault(a => a.AddressId == addressDto.AddressId);

                    address.Street = addressDto.Street;
                    address.State = addressDto.State;
                    address.House = addressDto.House;
                    address.Country = addressDto.Country;

                    dbContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

    }
}