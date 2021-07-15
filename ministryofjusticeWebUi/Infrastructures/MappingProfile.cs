using AutoMapper;
using Microsoft.AspNet.Identity.EntityFramework;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeWebUi.Dtos;
using ministryofjusticeWebUi.Models;

namespace ministryofjusticeWebUi.Infrastructures
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            Mapper.CreateMap<Department, DepartmentViewModel>().ReverseMap();
            Mapper.CreateMap<Case, CaseViewModel>().ReverseMap();
            Mapper.CreateMap<Case, CaseDetailsViewModel>().ReverseMap();
            Mapper.CreateMap<ProfileViewModel, ApplicationUser>().ReverseMap();
            Mapper.CreateMap<ProfileViewModel, Lawyer>();
            Mapper.CreateMap<Case, AssignCaseViewModel>().ReverseMap();
            Mapper.CreateMap<CreateAccountViewModel, ApplicationUser>();
     
            Mapper.CreateMap<Comment, CommentDto>().ReverseMap();
            Mapper.CreateMap<Comment, CommentViewModel>().ReverseMap();
            Mapper.CreateMap<UploadFileViewModels, File>().ReverseMap();

            Mapper.CreateMap<Notification, NotificationViewModel>();
            Mapper.CreateMap<IdentityRole, RoleViewModel>().ReverseMap();
        }
    }
}