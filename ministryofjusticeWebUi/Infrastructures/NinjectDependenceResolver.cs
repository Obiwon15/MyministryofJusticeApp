using Ninject;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Services.Description;
using ministryofjusticeDomain.IdentityEntities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeDomain.Interfaces.Services;
using ministryofjusticeDomain.Repositories;
using ministryofjusticeDomain.Services;
using ministryofjusticeWebUi.Interfaces;
using ministryofjusticeWebUi.Services;
using Ninject.Web.Common;

namespace ministryofjusticeWebUi.Infrastructures
{
    public class NinjectDependenceResolver : IDependencyResolver
    {
        private readonly IKernel kernel;

        public NinjectDependenceResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            kernel.Bind(typeof(ApplicationDbContext)).ToSelf();
            kernel.Bind<IUserUnitOfWork>().To<UserUnitOfWork>();
            kernel.Bind<ICaseUnitOfWork>().To<CaseUnitOfWork>();
            kernel.Bind<INotificationUnitOfWork>().To<NotificationUnitOfWork>();
            kernel.Bind<IDepartmentUnitOfWork>().To<DepartmentUnitOfWork>();
            kernel.Bind<IReportUnitOfWork>().To<ReportUnitOfWork>();

            //Repository services
            kernel.Bind<IDepartmentRepo>().To<DepartmentRepo>();
            kernel.Bind<IUserManagerRepo>().To<UserManagerRepo>();
            kernel.Bind<IProfileRepo>().To<ProfileRepo>();
            kernel.Bind<ICaseRepo>().To<CaseRepo>();
            kernel.Bind<ILawyerRepo>().To<LawyerRepo>();
            kernel.Bind<ICaseFileRepo>().To<CaseFileRepo>();
            kernel.Bind<ICommentRepo>().To<CommentRepo>();
            kernel.Bind<INotificationRepo>().To<NotificationRepo>();
            kernel.Bind<IDepartmentHeadRepo>().To<DepartmentHeadRepo>();

            // Utility services
            kernel.Bind<IMailSender>().To<MailSender>();
            kernel.Bind<ICustomFileSystem>().To<CustomFileSystem>();
            kernel.Bind<IRoleService>().To<RoleService>();

            // Controller services
            kernel.Bind<IReportService>().To<ReportService>();
            kernel.Bind<ICaseFileServices>().To<CaseFileServices>();
            kernel.Bind<IDepartmentServices>().To<DepartmentServices>();
            kernel.Bind<IUserServices>().To<UserServices>();
            kernel.Bind<ICaseService>().To<CaseService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<INotificationService>().To<NotificationService>();
        }
    }
}