using VModels.DTOS.Report;

namespace Core.Services;

public class SchoolService : ISchoolService
{
    private readonly ISeasonRepo _seasonRepo;
    private readonly ISchoolRepo _schoolsRepo;
    private readonly IMapper _mapper;

    public SchoolService(ISeasonRepo seasonRepo, ISchoolRepo schoolsRepo, IMapper mapper)
    {
        _seasonRepo = seasonRepo;
        _schoolsRepo = schoolsRepo;
        _mapper = mapper;
    }

    public Response<List<GetSchoolDto>> GetAll(int pageNumber, int pageSize)
    {
        var modelItems = _schoolsRepo.GetTableNoTracking().Include(m => m.Organization);
        var result = PaginatedList<GetSchoolDto>.Create(_mapper.Map<List<GetSchoolDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetSchoolDto>>(result));
    }

    public async Task<Response<GetSchoolReportDto>> GetSchoolReport(int schoolId, int SeasonId)
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
                                        UserEmail = userclass.User!.Email!,
                                        UserName = userclass.User.Name,
                                        Gender = userclass.User.Gender,
                                        NationalID = userclass.User.NationalID,
                                        ProfilePicturePath = userclass.User.ProfilePicturePath,
                                        UserTypeId = userclass.UserTypeId
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

        return ResponseHandler.Success(dto);
    }

    public Response<List<GetSchoolDto>> GetByOrganization(int orgId, int pageNumber, int pageSize)
    {
        var modelItems = _schoolsRepo
            .GetTableNoTracking()
            .Include(m => m.Organization)
            .Where(s => s.OrganizationId == orgId);

        var result = PaginatedList<GetSchoolDto>.Create(_mapper.Map<List<GetSchoolDto>>(modelItems), pageNumber, pageSize);

        return ResponseHandler.Success(_mapper.Map<List<GetSchoolDto>>(result));
    }

    public async Task<Response<GetSchoolDto?>> GetById(int id)
    {
        var modelItem = await _schoolsRepo.GetByIdAsync(id);
        if (modelItem == null)
            return null;
        return ResponseHandler.Success(_mapper.Map<GetSchoolDto>(modelItem))!;
    }

    public async Task<Response<GetSchoolDto>> Add(AddSchoolDto dto)
    {
        var modelItem = _mapper.Map<School>(dto);

        var model = await _schoolsRepo.AddAsync(modelItem);

        return ResponseHandler.Created(_mapper.Map<GetSchoolDto>(modelItem));
    }

    public async Task<Response<bool>> Update(UpdateSchoolDto dto)
    {
        var modelItem = await _schoolsRepo.GetByIdAsync(dto.Id);

        if (modelItem is null)
            return ResponseHandler.NotFound<bool>();

        _mapper.Map(dto, modelItem);

        var model = _schoolsRepo.UpdateAsync(modelItem);

        return ResponseHandler.Success(true);
    }

    public async Task<Response<bool>> Delete(int id)
    {

        var dbModel = await _schoolsRepo.GetByIdAsync(id);

        if (dbModel == null)
            return ResponseHandler.NotFound<bool>();

        await _schoolsRepo.DeleteAsync(dbModel);
        return ResponseHandler.Deleted<bool>();
    }
}
