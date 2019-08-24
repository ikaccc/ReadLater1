$(() => {
    $("#add-category-form").submit((e) => {
        e.preventDefault();
        $.ajax({
            type: "POST",
            url: "/Categories/CreateAjax",
            dataType: "JSON",
            data: $("#add-category-form").serialize(),
            success: function(result) {
                if (result.isValid) {
                    $("#CategoryId").append("<option selected value='" +
                        result.newCategory.ID +
                        "'>" +
                        result.newCategory.Name +
                        "</option>");
                    $("#add-category-modal").modal("hide");
                    $("#add-category-form")[0].reset();
                } else {
                    $(".validation-summary-valid ul").empty();
                    $(".validation-summary-valid ul").append("<li>" + result.errorMessage + "</li>");
                }
            },
            error: function(x, y, z) {
                alert("error");
            }
        });
    });
});

