$(document).ready(function () {
    $('#OrganizationId').on('change', function () {
        var selectedOrganizationId = $(this).val();
        var schoolDropdown = $('#SchoolId');

        schoolDropdown.empty();

        schoolDropdown.append($('<option>').val('').text('Select School'));
         $.ajax({
             url: '/Courses/GetSchoolsByOrganization',
             type: 'GET',
             data: { organizationId: selectedOrganizationId },
             success: function (data) {
        debugger;
                 $.each(data, function (i, item) {
                     schoolDropdown.append($('<option>').val(item.value).text(item.text));
                 });
             }
         });
    });
});
