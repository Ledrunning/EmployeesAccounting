﻿using System;
using System.Threading;
using System.Windows;
using Autofac;
using EA.DesktopApp.Contracts;
using EA.DesktopApp.Contracts.ViewContracts;
using EA.DesktopApp.Rest;
using EA.DesktopApp.Services;
using EA.DesktopApp.Services.ViewServices;
using EA.DesktopApp.View;
using EA.DesktopApp.ViewModels;
using EA.RecognizerEngine.Contracts;
using EA.RecognizerEngine.Engines;
using Emgu.CV.Face;
using NLog;

namespace EA.DesktopApp.DiSetup
{
    public static class AutofacConfigure
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public static IContainer Container { get; private set; }
        
        public static void ConfigureContainer()
        {
            try
            {
                var builder = new ContainerBuilder();

                var appConfig = new AppConfig();

                var loadedConfiguration = appConfig.LoadConfiguration();

                var adminGatewayService = new AdminGatewayService(loadedConfiguration);
                var employeeGatewayService = new EmployeeGatewayService(loadedConfiguration);

                // Register the CancellationTokenSource as a single instance so the same source is used everywhere.
                builder.RegisterInstance(new CancellationTokenSource()).AsSelf();
                // Register a factory to get the CancellationToken from the source.
                builder.Register(c => c.Resolve<CancellationTokenSource>().Token).As<CancellationToken>();

                builder.RegisterType<WindowFactory>().As<IWindowFactory>().SingleInstance();
                builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();

                builder.RegisterType<ModalViewModelFactory>().As<IModalViewModelFactory>().SingleInstance();

                //InitializeLpbhRecognizer(builder);

                builder.RegisterType<EigenFaceRecognizer>().SingleInstance();
                builder.RegisterType<EigenFaceRecognition>().As<IEigenFaceRecognition>().SingleInstance();
                builder.RegisterType<FaceDetectionService>().As<IFaceDetectionService>().InstancePerLifetimeScope();
                builder.RegisterType<PhotoShootService>().As<IPhotoShootService>().InstancePerLifetimeScope();
                builder.RegisterType<SoundPlayerService>().As<ISoundPlayerService>().InstancePerLifetimeScope();
                builder.RegisterInstance(employeeGatewayService).As<IEmployeeGatewayService>().SingleInstance();
                builder.RegisterInstance(adminGatewayService).As<IAdminGatewayService>().SingleInstance();
                
                builder.RegisterType<MainWindow>().InstancePerDependency();
                builder.RegisterType<MainViewModel>().InstancePerDependency();

                builder.RegisterType<AdminForm>().InstancePerDependency();
                builder.RegisterType<AdminViewModel>().InstancePerDependency();

                builder.RegisterType<Window>().InstancePerDependency(); 
                builder.RegisterType<ModalWindow>().InstancePerDependency(); 
                builder.RegisterType<ModalViewModel>().InstancePerDependency(); 

                builder.RegisterType<LoginWindow>().InstancePerDependency(); 
                builder.RegisterType<LoginViewModel>().InstancePerDependency();

                builder.RegisterType<RedactorForm>().InstancePerDependency();
                builder.RegisterType<RedactorViewModel>().InstancePerDependency();
                
                builder.RegisterType<RegistrationForm>().InstancePerDependency();
                builder.RegisterType<RegistrationViewModel>().InstancePerDependency();

                Container = builder.Build();
            }
            catch (Exception e)
            {
                Logger.Error("Application run error! {e}", e);
                throw new ApplicationException($"{e.Message}. Application run error!");
            }
        }

        private static void InitializeLpbhRecognizer(ContainerBuilder builder)
        {
            // Register the LBPHFaceRecognizer type with appropriate parameters
            builder.Register(c => new LBPHFaceRecognizer(1, 8, 8, 8, 400.00))
                .As<LBPHFaceRecognizer>()
                .SingleInstance(); // You can choose the appropriate lifetime scope

            // Register the LbphFaceRecognition class and specify its dependencies
            builder.RegisterType<LbphFaceRecognition>()
                .As<ILbphFaceRecognition>()
                .WithParameter(
                    (pi, c) => pi.ParameterType == typeof(LBPHFaceRecognizer),
                    (pi, c) => c.Resolve<LBPHFaceRecognizer>());
        }
    }
}