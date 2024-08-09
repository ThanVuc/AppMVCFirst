// Can be optimize in Selector to flexible
function AutoUploadFile() {
    var item = document.getElementById("selectfileupload");
    var formData = new FormData();

    var inp = $("#selectfileupload");

    var id = inp.data("id");
    var url = inp.data("url");

    formData.append("id", id);

    var fileNums = item.files.length;
    if (fileNums == 0) return;
    var fileData = item.files[0];
    formData.append("UploadImage", fileData);

    var urlUploadImage = url;

    $.ajax({
        data: formData,
        cache: false,
        url: urlUploadImage,
        type: "POST",
        contentType: false,
        processData: false,
        success: function (data) {
            LoadImage();
        }
    });

}

function ClickButtonUpload() {
    $("#selectfileupload").click();
}

function SetClickDeleteImage() {
    $("#box-image-upload .product-image .delete-image").click(function () {
        event.preventDefault();
        if (confirm("Are you sure to delete the photo?") != true) return;
        var deleteButton = $(this);
        var id = deleteButton.data("id");
        var url = deleteButton.data("deleteurl");

        var formData = new FormData();
        formData.append("id", id);

        var urlDeleteImage = url;

        $.ajax({
            data: formData,
            cache: false,
            url: urlDeleteImage,
            type: "POST",
            contentType: false,
            processData: false,
            success: function (data) {
                LoadImage();
            }
        });

    });
}

function LoadImage() {
    var box = $("#box-image-upload");
    var productID = box.data("id");
    var url = box.data("url");
    var deleteUrl = box.data("deleteurl");

    box.empty();
    var formData = new FormData();
    formData.append("id", productID);

    var urlListImage = url;

    $.ajax({
        data: formData,
        cache: false,
        url: urlListImage,
        type: "POST",
        contentType: false,
        processData: false,
        success: function (data) {
            data.images.forEach(function (image) {
                var e = $(
                    '<div class="product-image p-1">'
                    + '<img class= "w-100" src = "' + image.path + '" alt = "Err" />'
                    + '<button class="btn btn-danger delete-image" data-deleteurl="' + deleteUrl +'" data-id="' + image.id + '" >Delete</button> </div>'
                );
                box.append(e);
            })
            SetClickDeleteImage();
        }
    });

}