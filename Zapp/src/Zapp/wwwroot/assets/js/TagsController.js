var tagList = [];
var tagCounter = 1;


function enterKeyPress(e) {
    if (e.keyCode == 13) {
        addTag();
        return false;
    }
}

function addTag() {
    var inputValue = $("#newTag").val();
    var id = tagCounter;

    // Checks if the inputted value contains anything other than whitespace/empty
    if (/\S/.test(inputValue)) {
        var newButton = document.createElement("label");
        var newSpan = document.createElement("button");
        tagList.push(inputValue);

        $("#tags")
            .append(newSpan);
        newSpan.appendChild(newButton);

        $(newButton)
            .attr("id", "tag" + id)
        .css("font-family", "Varela Round");

        newButton.innerHTML = inputValue;

        $(newSpan)
            .addClass("glyphicon glyphicon-remove")
            .addClass("btn btn-tag btn-sm")
            .addClass("btnConfig")
            .attr("id", "btnRemove" + id)
            .css("color", "grey")
            .css("font-size", "12px")
            .css("cursor", "pointer")
            .click(function () {
                removeTag(id);
            });

        tagCounter++;

        $("#newTag").val("");

        $("#hiddenTags").val(tagList.join(','));
    }

    function removeTag(id) {
        var tagSelector = "#tag" + id;
        var spanSelector = "#btnRemove" + id;
        $(tagSelector).remove();
        $(spanSelector).remove();
        tagCounter--;
    }
}