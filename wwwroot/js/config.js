
var x = $('#ArtID').val();
CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = 'html5video,widget,widgetselection,clipboard,lineutils';
    
    config.removeDialogTabs = 'image:advanced;image:Link;link:advanced;link:upload';
    config.filebrowserImageUploadUrl = '/Admin/Articles/UploadImage?id=' + x; //Action for Uploding image

   
    config.filebrowserBrowseUrl = '/Resources/' + x;
    config.filebrowserUploadUrl = '/Admin/Articles/UploadFile?id=' + x; //Action for Uploding file
    
    
};

