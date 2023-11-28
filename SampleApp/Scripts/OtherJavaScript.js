
// Load Data in Table when document is ready
$(document).ready(function () {
    loadData();
    // Modal event when modal close
    $('#myModal').on('hidden.bs.modal', function () {
        clearTextBox();
        clearValidate();

    });
});

// Function for clearing the textboxes
function clearTextBox() {
    //$('#EmployeeID').val("");
    $('#PetName').val("");
    $('#PetAge').val("");
    // Clear the select input
    $('#PetGender').val("Choose...");
    $('#imagePreview').attr("src", "AppFiles/Images/default.png");
    // For file input
    var fileInput = $('#fileUpload');
    fileInput.val('');  // Clear the value of the file input
    $('#btnUpdate').hide();
    $('#btnAdd').show();
    clearValidate();
}


function clearValidate() {
    $('#PetName').css('border-color', 'lightgrey');
    $('#PetAge').css('border-color', 'lightgrey');
    $('#PetGender').css('border-color', 'lightgrey');
    $('#imagePreview').css('border-color', 'lightgrey');

    $('#PetNameError').css('color', 'lightgrey');
    $('#PetAgeError').css('color', 'lightgrey');
    $('#PetGenderError').css('color', 'lightgrey');
    $('#imagePreview').css('color', 'lightgrey');
    $('#fileUploadError').css('color', 'lightgrey');
}


//Image preview function
function showImagePreview(imageUploader, previewImage) {
    if (imageUploader.files && imageUploader.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $(previewImage).attr('src', e.target.result);
        }
        reader.readAsDataURL(imageUploader.files[0]);
    }
}



// Load Data function
function loadData() {
    $.ajax({
        url: "/Home/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.PetID + '</td>';
                html += '<td>' + item.PetName + '</td>';
                html += '<td>' + item.PetAge + '</td>';
                html += '<td>' + item.PetGender + '</td>';
                // Display the image using an img tag
                // Check if PetImagePath is not empty
                // Check if PetImagePath start with ~ remove it
                // Set the image preview
                var imagePreview = $('#imagePreview');
                var defaultImagePath = '~/AppFiles/Images/default.png';

                var imagePath = item.PetImagePath.startsWith("~") ? item.PetImagePath.substring(1) : item.PetImagePath;
                var resolvedImagePath = imagePath;
                if (item.PetImagePath.startsWith("~") ? item.PetImagePath.substring(1) : item.PetImagePath) {
                    html += '<td><img src="' + resolvedImagePath + '" alt="Pet Image" style="max-width: 50px; max-height: 50px;"></td>';
                } else {
                    html += '<td><img src="' + defaultImagePath + '" alt="Default Image" style="max-width: 50px; max-height: 50px;"></td>';
                }
                html += '<td><a href="#" class="btn btn-info btn-sm" onclick="return getbyID(' + item.PetID + ')"><i class="fa fa-pencil fa-lg"></i>Edit</a> | <a href="#" class="btn btn-danger btn-sm" onclick="return Delele(' + item.PetID + ')"><i class="fa fa-trash fa-lg"></i>Delete</a></td>';
                html += '</tr>';
            });
            $('#petTableBody').html(html);
            toastr.success("Successfully listing the pet");
        },
        error: function (errormessage) {
            toastr.error("An error occurred while listing the pet: " + error);
            //alert(errormessage.responseText);
        }
    });
}

// Add Data Function
function Add() {
    var res = validate();
    if (res == false) {
        return false;
    }

    // Create a FormData object
    var formData = new FormData();
    formData.append('PetID', $('#PetID').val());
    formData.append('PetName', $('#PetName').val());
    formData.append('PetAge', $('#PetAge').val());
    formData.append('PetGender', $('#PetGender').val());
    formData.append("PetImagePath", $("#fileUpload")[0].files[0]);

    $.ajax({
        url: "/Home/Add",
        data: formData,
        type: "POST",
        contentType: false,
        processData: false,
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            clearTextBox();
            toastr.success(result.message);
        },
        error: function (errormessage) {
            toastr.error(result.message);
            //alert(errormessage.responseText);
        }
    });
}



// Function for getting the Data Based upon Employee ID
function getbyID(PetsID) {
    $('#PetName').css('border-color', 'lightgrey');
    $('#PetAge').css('border-color', 'lightgrey');
    $('#PetGender').css('border-color', 'lightgrey');

    $('#PetNameError').css('color', 'lightgrey');
    $('#PetAgeError').css('color', 'lightgrey');
    $('#PetGenderError').css('color', 'lightgrey');
    $('#imagePreview').css('color', 'lightgrey');
    $('#fileUploadError').css('color', 'lightgrey');

    $.ajax({
        url: "/Home/getbyID/" + PetsID,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#PetID').val(result.PetID);
            $('#PetName').val(result.PetName);
            $('#PetAge').val(result.PetAge);
            $('#PetGender').val(result.PetGender);

            // Set the image preview
            var imagePreview = $('#imagePreview');
            var defaultImagePath = '~/AppFiles/Images/default.png';

            // Check if PetImagePath start with ~ remove it
            var imagePath = result.PetImagePath.startsWith("~") ? result.PetImagePath.substring(1) : result.PetImagePath;
            var resolvedImagePath = imagePath;

            if (result.PetImagePath.startsWith("~") ? result.PetImagePath.substring(1) : result.PetImagePath) {
                imagePreview.attr('src', resolvedImagePath);
            } else {
                imagePreview.attr('src', defaultImagePath);
            }

            $('#myModal').modal('show');
            $('#btnUpdate').show();
            $('#btnAdd').hide();

            toastr.success("Successfully geting the pet for update");
        },
        //error: function (xhr, status, error) {
        //    // Handle the case where the AJAX request encounters an error
        //    toastr.error("An error occurred while updating the pet: " + error);
        //}
        error: function (errormessage) {
            toastr.error("An error occurred while geting the pet for update: " + error);
            //alert(errormessage.responseText);
        }
    });
    return false;
}

// Function for updating pet's record
function Update() {
    //var res = validate();
    //if (res == false) {
    //    return false;
    //}
    var formData = new FormData();
    formData.append('PetID', $('#PetID').val()); // Include the PetID for update
    formData.append('PetName', $('#PetName').val());
    formData.append('PetAge', $('#PetAge').val());
    formData.append('PetGender', $('#PetGender').val());
    formData.append("PetImagePath", $("#fileUpload")[0].files[0]);

    $.ajax({
        url: "/Home/Update",
        data: formData,
        type: "POST",
        contentType: false,
        processData: false,
        success: function (result) {
            loadData();
            $('#myModal').modal('hide');
            $('#PetName').val("");
            $('#PetAge').val("");
            $('#PetGender').val("");
            $('#imagePreview').attr("src", "AppFiles/Images/default.png");
            toastr.success(result.message);
        },

        error: function (errormessage) {
            toastr.error(errormessage.message);
            //alert(errormessage.responseText);
        }
    });

}


// Function for deleting pet's record
// Function for deleting pet's record
function Delele(ID) {
    // Show the SweetAlert confirmation dialog
    Swal.fire({
        title: 'Are you sure?',
        text: 'You won\'t be able to revert this!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        // Check if the user clicked "Yes"
        if (result.isConfirmed) {
            // If the user clicks "Yes," proceed with the deletion
            $.ajax({
                url: "/Home/Delete/" + ID,
                type: "POST",
                contentType: "application/json;charset=UTF-8",
                dataType: "json",
                success: function (ajaxResult) {
                    if (ajaxResult.success) {
                        // Reload data and show success message only if the user clicked "Yes"
                        loadData();
                        toastr.success("Successfully deleted the pet data.");
                    } else {
                        // Show error message
                        Swal.fire({
                            title: 'Error!',
                            text: ajaxResult.message,
                            icon: 'error'
                        });
                    }
                },
                error: function (errormessage) {
                    toastr.error("An error occurred while getting the pet for delete: " + error);
                    //alert(errormessage.responseText);
                }
            });
        }
    });
}


//Valdidation using jquery
function validate() {
    var isValid = true;

    // Reset all border colors and hide error labels
    $('.form-control').css('border-color', 'lightgrey');
    $('.error-label').hide();

    // Validate PetName
    if ($('#PetName').val().trim() == "") {
        $('#PetName').css('border-color', 'Red');
        //$('#PetNameError').css('color', 'Red');
        isValid = false;
    }

    // Validate PetAge
    if ($('#PetAge').val().trim() == "") {
        $('#PetAge').css('border-color', 'Red');
        //$('#PetAgeError').css('color', 'Red');
        isValid = false;
    }

    // Validate PetGender
    if ($('#PetGender').val().trim() == "" || $('#PetGender').val() == "Choose...") {
        $('#PetGender').css('border-color', 'Red');
        //$('#PetGenderError').css('color', 'Red');
        isValid = false;
    }

    // Validate Image (file upload)
    var imageInput = $('#fileUpload')[0];
    if (imageInput.files.length === 0) {
        $('#fileUpload').css('border-color', 'Red');
        //$('#fileUploadError').css('color', 'Red');
        isValid = false;
    }

    return isValid;
}




