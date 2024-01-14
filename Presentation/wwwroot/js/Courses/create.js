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

function toggleList() {
    
    var selectedContentType = $('#ContentType').val();;
    var contentInputDiv = $('#contentInput');
    var attachmentInputDiv = $('#attachmentInput');

    contentInputDiv.hide();
    attachmentInputDiv.hide();


    if (selectedContentType === 'Link' || selectedContentType === 'Text') {
        contentInputDiv.show();
        attachmentInputDiv.hide();
    } else if (selectedContentType === 'Attachment') {
        contentInputDiv.hide();
        attachmentInputDiv.show();
    } else {
        contentInputDiv.hide();
        attachmentInputDiv.hide();
    }
};
$('#ContentType').on('change', toggleList);