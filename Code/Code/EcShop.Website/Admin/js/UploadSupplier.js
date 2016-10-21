var queueErrorArray;
var swfu1;
var swfu2;
var swfu3;

window.onload = function () {
    swfu1 = new SWFUpload({
        // Backend Settings
        upload_url: "../UploadHandler.ashx",
        use_query_string: true,
        post_params: {
            action: "upload",
            AUTHID: auth,
            uploadPath: "/Storage/master/banner/"
        },

        // File Upload Settings
        file_size_limit: "1024",
        file_types: "*.jpg;*.jpeg;*.png;*.bmp;*.gif",
        file_types_description: "All Image Files",
        file_upload_limit: 0,    // Zero means unlimited

        // Event Handler Settings - these functions as defined in Handlers.js
        // The handlers are not part of SWFUpload but are part of my website and control how
        // my website reacts to the SWFUpload events.
        file_queue_error_handler: fileQueueError,
        file_dialog_complete_handler: fileDialogComplete,
        upload_progress_handler: uploadProgress,
        upload_error_handler: uploadError,
        upload_success_handler: uploadSuccess1,
        upload_complete_handler: uploadComplete,

        // Button settings
        button_image_url: "../images/upload.jpg",

        // button_text: "点击开始上传",
        button_placeholder_id: "spanButtonPlaceholder1",
        button_width: 100,
        button_height: 22,
        button_cursor: SWFUpload.CURSOR.HAND,
        button_action: SWFUpload.BUTTON_ACTION.SELECT_FILE, //只能选择一个文件

        // Flash Settings
        flash_url: "../js/swfupload/swfupload.swf", // Relative to this file
        custom_settings: {
            upload_target: "divFileProgressContainer1"
        }
    });


    swfu2 = new SWFUpload({
        // Backend Settings
        upload_url: "../UploadHandler.ashx",
        use_query_string: true,
        post_params: {
            action: "upload",
            AUTHID: auth,
            uploadPath: "/Storage/master/banner/"
        },

        // File Upload Settings
        file_size_limit: "1024",
        file_types: "*.jpg;*.jpeg;*.png;*.bmp;*.gif",
        file_types_description: "All Image Files",
        file_upload_limit: 0,    // Zero means unlimited

        // Event Handler Settings - these functions as defined in Handlers.js
        // The handlers are not part of SWFUpload but are part of my website and control how
        // my website reacts to the SWFUpload events.
        file_queue_error_handler: fileQueueError,
        file_dialog_complete_handler: fileDialogComplete,
        upload_progress_handler: uploadProgress,
        upload_error_handler: uploadError,
        upload_success_handler: uploadSuccess2,
        upload_complete_handler: uploadComplete,

        // Button settings
        button_image_url: "../images/upload.jpg",

        // button_text: "点击开始上传",
        button_placeholder_id: "spanButtonPlaceholder2",
        button_width: 100,
        button_height: 22,
        button_cursor: SWFUpload.CURSOR.HAND,
        button_action: SWFUpload.BUTTON_ACTION.SELECT_FILE, //只能选择一个文件

        // Flash Settings
        flash_url: "../js/swfupload/swfupload.swf", // Relative to this file
        custom_settings: {
            upload_target: "divFileProgressContainer2"
        }
    });


    swfu3 = new SWFUpload({
        // Backend Settings
        upload_url: "../UploadHandler.ashx",
        use_query_string: true,
        post_params: {
            action: "upload",
            AUTHID: auth,
            uploadPath: "/Storage/master/banner/"
        },

        // File Upload Settings
        file_size_limit: "1024",
        file_types: "*.jpg;*.jpeg;*.png;*.bmp;*.gif",
        file_types_description: "All Image Files",
        file_upload_limit: 0,    // Zero means unlimited

        // Event Handler Settings - these functions as defined in Handlers.js
        // The handlers are not part of SWFUpload but are part of my website and control how
        // my website reacts to the SWFUpload events.
        file_queue_error_handler: fileQueueError,
        file_dialog_complete_handler: fileDialogComplete,
        upload_progress_handler: uploadProgress,
        upload_error_handler: uploadError,
        upload_success_handler: uploadSuccess3,
        upload_complete_handler: uploadComplete,

        // Button settings
        button_image_url: "../images/upload.jpg",

        // button_text: "点击开始上传",
        button_placeholder_id: "spanButtonPlaceholder3",
        button_width: 100,
        button_height: 22,
        button_cursor: SWFUpload.CURSOR.HAND,
        button_action: SWFUpload.BUTTON_ACTION.SELECT_FILE, //只能选择一个文件

        // Flash Settings
        flash_url: "../js/swfupload/swfupload.swf", // Relative to this file
        custom_settings: {
            upload_target: "divFileProgressContainer3"
        }
    });
}