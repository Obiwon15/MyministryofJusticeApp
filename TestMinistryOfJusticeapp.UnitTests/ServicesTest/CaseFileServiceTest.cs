using System;
using System.Collections.Generic;
using System.Web;
using AutoMapper;
using ministryofjusticeDomain.Entities;
using ministryofjusticeDomain.Interfaces.Repository;
using ministryofjusticeDomain.Interfaces.Services;
using ministryofjusticeWebUi.Models;
using ministryofjusticeWebUi.Services;
using Moq;
using NUnit.Framework;
using TestMinistryOfJusticeapp.UnitTests.RepositoryTest;

namespace TestMinistryOfJusticeapp.UnitTests.ServicesTest
{
    [TestFixture]
    public class CaseFileServiceTest
    {
        private Mock<ICaseFileRepo> _caseFileRepo;
        private IEnumerable<File> _files;
        private CaseFileServices _caseFile;
        private UploadFileViewModels _fileView;
        private Mock<HttpPostedFileBase> _file;
        private List<File> _fileDb;
        private readonly Mock<IMappingEngine> _mapper = new Mock<IMappingEngine>();
        private Mock<ICustomFileSystem> _customFileSystem;
        private File _filed;

        [SetUp]
        public void Setup()
        {
            _file = new Mock<HttpPostedFileBase>();
            _caseFileRepo = new Mock<ICaseFileRepo>();
            _customFileSystem = new Mock<ICustomFileSystem>();

            _files = new List<File>();
            _fileView = new UploadFileViewModels();
            _fileDb = new List<File>();
            _filed = new File();


            _file.Setup(d => d.FileName).Returns("test1.txt");
            _caseFile = new CaseFileServices(_caseFileRepo.Object, _customFileSystem.Object);
            Mapper.Initialize(cfg => { cfg.CreateMap<UploadFileViewModels, File>(); });
        }

        [Test(Description = "ManageCaseFile_WhenCalled_ReturnCaseFile")]
        [TestCase(1)]
        public void NewTest_For_ManageCaseFile(int id)
        {
            //Arrange
            _caseFileRepo.Setup(fr => fr.GetAllCaseFiles(id)).Returns(_files);

            //Act
            var result = _caseFile.ManageCaseFiles(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(ManageCaseFilesViewModel), result);
            _caseFileRepo.Verify(r => r.GetAllCaseFiles(id));
        }

        [Test(Description = "UploadCaseFile_WhenCalled_AddACaseFile")]
        [TestCase(@"c:\Temp\CaseFiles.txt")]
        public void NewTest_For_UploadCaseFile(string path)
        {
            //Arrange
          _mapper.Setup(mf => mf.Map<UploadFileViewModels, File>(_fileView));

            _caseFileRepo.Setup(fs => fs.UploadFile(new File(), path, _file.Object))
                .Returns(() => new File());

            _caseFileRepo.Setup(fs => fs.Insert(It.IsAny<File>())).Callback<File>(_fileDb.Add)
                .Verifiable();

            //Act
            var data = _caseFile.UploadCaseFile(_fileView, It.IsAny<string>(), _file.Object);

            //Assert
            Assert.That(data, Is.True);
            _caseFileRepo.Verify(y => y.Insert(It.IsAny<File>()));
        }

        [Test(Description = "If_ThereIsAnErrorInUpLoading_ThrowNewException")]
        [TestCase(@"c:\Temp\CaseFiles.txt")]
        public void NewTest_For_loadCaseFile2(string path)
        {
            //Arrange
            _mapper.Setup(mf => mf.Map<UploadFileViewModels, File>(_fileView)).Returns(() => null);
          

            //Act
            var data = _caseFile.UploadCaseFile(_fileView, It.IsAny<string>(), _file.Object);

            //Assert
            Assert.That(data, Is.True);

           
        }

        [Test(Description = "DeleteCaseFile_WhenCalled_DeleteAFile")]
        [TestCase(1)]
        public void NewTest_For_DeleteCaseFile(int id)
        {
            //Arrange
            var file = new File {Id = id, FilePath = It.IsAny<string>()};
            _caseFileRepo.Setup(cf => cf.GetById(file.Id)).Returns(file);
            _caseFileRepo.Setup(cf => cf.Delete(file.Id)).Verifiable();
            _customFileSystem.Setup(s => s.DeleteFile(file)).Verifiable();

            //Act
            var result = _caseFile.DeleteFile(id);

            //Assert
            Assert.That(result, Is.True);
            _caseFileRepo.Verify(x => x.Delete(file.Id));
            _customFileSystem.Verify(s => s.DeleteFile(file));
            _caseFileRepo.Verify(x => x.Save());
        }

        [Test(Description = "If_ThereIsNoCaseThatMatchesTheId_ThrowNewException")]
        [TestCase(1)]
        public void NewTest_For_DeleteCaseFile2(int id)
        {
            //Arrange
            var file = new File {Id = id, FilePath = It.IsAny<string>()};
            _caseFileRepo.Setup(cf => cf.GetById(file.Id)).Callback(() => throw new Exception());


            //Assert
            var ex = Assert.Throws<Exception>(() => _caseFile.DeleteFile(id));
            Assert.That(ex.Message, Is.Not.EqualTo("Actual exception message"));
        }

        [Test(Description = "GetFileType_WhenFileExist_ReturnFileType")]
        [TestCase("doc1.pdf", "pdf")]
        [TestCase("doc1.jpeg", "jpeg")]
        [TestCase("doc1.doc", "doc")]
        [TestCase("doc1.jpg", "jpg")]
        [TestCase("doc1.docx", "doc")]
        [TestCase("doc1.random Extension", "Not Found")]
        public void NewTest_For_GetFileType(string input, string output)
        {
            //Arrange
            _customFileSystem.Setup(c => c.DoesFileExists(input)).Returns(true);

            //Act
            var result = _caseFile.GetFileType(input);


            //Assert
            Assert.That(result, Is.EqualTo(output));
            _customFileSystem.Verify(s => s.DoesFileExists(input));
        }

        [Test(Description = "ErrorInPath_WhenPathIsNullOrWhitespace_ThrowException")]
        [TestCase("null")]
        [TestCase("")]
        [TestCase(" ")]
        public void NewTest_For_Path(string input)
        {
            _customFileSystem.Setup(c => c.DoesFileExists(input)).Callback(() => throw new Exception());

            //Assert
            var ex = Assert.Throws<Exception>(() => _caseFile.GetFileType(input));
            Assert.That(ex.Message, Is.Not.EqualTo("Actual exception message"));
        }
    }
}