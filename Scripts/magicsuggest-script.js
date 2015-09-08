$(function() {

    var ms =  $('#ms').magicSuggest({
        data: 'jsonkeyword.aspx',
        valueField: 'id',
        displayField: 'title',
		maxSelection: 1
    });
    $(ms).on('selectionchange', function () {
        $("#itemLabel").val(JSON.stringify(this.getSelection()[0]["title"]));
    });
});