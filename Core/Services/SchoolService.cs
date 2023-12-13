using VModels.DTOS.Report;

namespace Core.Services;

public class SchoolService : ISchoolService
{
    private readonly ISeasonRepo _seasonRepo;
    private readonly ISchoolRepo _schoolsRepo;
    private readonly BaseSettings _baseSettings;
    private readonly IMapper _mapper;

    public SchoolService(
        ISeasonRepo seasonRepo,
        ISchoolRepo schoolsRepo,
        BaseSettings baseSettings,
        IMapper mapper)
    {
        _seasonRepo = seasonRepo;
        _schoolsRepo = schoolsRepo;
        _baseSettings = baseSettings;
        _mapper = mapper;
    }

    public Result<PaginatedList<GetSchoolDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _schoolsRepo.GetTableNoTracking().Include(m => m.Organization);
        var result = PaginatedList<GetSchoolDto>.Create(_mapper.Map<List<GetSchoolDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(result);
    }

    public async Task<Result<GetSchoolReportDto>> GetSchoolReport(int schoolId, int SeasonId)
    {
        var model = await _schoolsRepo
            .GetTableNoTracking()
            .Include(m => m.Organization)
            .Include(m => m.Grades.Where(s => s.Classrooms
                .Any(c => c.UserClasses
                    .Any(uc => uc.SeasonId == SeasonId))))
            .ThenInclude(g => g.Classrooms)
            .ThenInclude(c => c.UserClasses.Where(uc => uc.SeasonId == SeasonId))
            .ThenInclude(uc => uc.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync(s => s.Id == schoolId);

        var seasonModel = await _seasonRepo.GetByIdAsync(SeasonId);

        var dto = new GetSchoolReportDto();
        if (model is not null)
        {
            dto.SchoolId = model.Id;
            dto.School = model.Name;
            dto.SchoolDescription = model.Description!;
            dto.Organization = model.Organization!.Name;
            dto.PicturePath = model.PicturePath;

            if (model.Seasons is not null)
            {
                dto.SeasonId = seasonModel.Id;
                dto.Season = seasonModel.Name;
                dto.From = seasonModel.From;
                dto.To = seasonModel.To;
                dto.IsCurrent = seasonModel.IsCurrent;
            }

            if (model.Grades.Any())
            {
                dto.Grades = new List<GradesDto>();
                foreach (var grade in model.Grades)
                {
                    var gradeDto = new GradesDto
                    {
                        Id = grade.Id,
                        Name = grade.Name,
                        Description = grade.Description,
                        Order = grade.Order
                    };
                    if (grade.Classrooms.Any())
                    {
                        gradeDto.Classrooms = new List<ClassroomDto>();
                        foreach (var classroom in grade.Classrooms)
                        {
                            var classDto = new ClassroomDto
                            {
                                Id = classroom.Id,
                                Name = classroom.Name,
                                Location = classroom.Location!,
                                Order = classroom.Order,
                                PicturePath = classroom.PicturePath,
                                StudentImagePath = classroom.StudentImagePath,
                                TeacherImagePath = classroom.TeacherImagePath,
                            };
                            if (classroom.UserClasses.Any())
                            {
                                classDto.Users = new List<UserDto>();
                                foreach (var userclass in classroom.UserClasses)
                                {
                                    classDto.Users.Add(new UserDto
                                    {
                                        Id = userclass.User!.Id,
                                        Email = userclass.User.Email!,
                                        UserName = userclass.User.Name,
                                        Gender = userclass.User.Gender,
                                        Name = userclass.User.Name,
                                        PhoneNumber = userclass.User.PhoneNumber!,
                                        Address = userclass.User.Address,
                                        Birthdate = userclass.User.Birthdate,
                                        PositionType = userclass.User.PositionType,
                                        SchoolUniversityJob = userclass.User.SchoolUniversityJob,
                                        UserTypeId = userclass.UserTypeId,
                                        FirstMobile = userclass.User.FirstMobile,
                                        SecondMobile = userclass.User.SecondMobile,
                                        FatherMobile = userclass.User.FatherMobile,
                                        MotherMobile = userclass.User.MotherMobile,
                                        MentorName = userclass.User.MentorName,
                                        GpsLocation = userclass.User.GpsLocation,
                                        Notes = userclass.User.Notes,
                                        ParticipationNumber = userclass.User.ParticipationNumber,
                                        NationalID = userclass.User.NationalID,
                                        ProfilePicturePath = $"{_baseSettings.url}/{_baseSettings.usersPath}/{userclass.User.ProfilePicturePath}"
                                    });
                                }
                            }
                            gradeDto.Classrooms.Add(classDto);
                        }
                    }
                    dto.Grades.Add(gradeDto);
                }
            }
        }

        return ResultHandler.Success(dto);
    }

    public Result<List<GetSchoolDto>> GetByOrganization(int orgId, int pageNumber, int pageSize)
    {
        var modelItems = _schoolsRepo
            .GetTableNoTracking()
            .Include(m => m.Organization)
            .Where(s => s.OrganizationId == orgId);

        var result = PaginatedList<GetSchoolDto>.Create(_mapper.Map<List<GetSchoolDto>>(modelItems), pageNumber, pageSize);

        return ResultHandler.Success(_mapper.Map<List<GetSchoolDto>>(result));
    }

    public async Task<Result<GetSchoolDto?>> GetById(int id)
    {
        var modelItem = await _schoolsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResultHandler.Success(_mapper.Map<GetSchoolDto>(modelItem))!;
    }

    public async Task<Result<GetSchoolDto>> Add(AddSchoolDto dto)
    {
        var modelItem = _mapper.Map<School>(dto);

        var model = await _schoolsRepo.AddAsync(modelItem);

        return ResultHandler.Created(_mapper.Map<GetSchoolDto>(modelItem));
    }

    public async Task<Result<bool>> Update(UpdateSchoolDto dto)
    {
        var modelItem = await _schoolsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResultHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _schoolsRepo.UpdateAsync(modelItem);

        return ResultHandler.Success(true);
    }

    public async Task<Result<bool>> Delete(int id)
    {

        var dbModel = await _schoolsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResultHandler.NotFound<bool>();

        await _schoolsRepo.DeleteAsync(dbModel);
        return ResultHandler.Deleted<bool>();
    }
}
