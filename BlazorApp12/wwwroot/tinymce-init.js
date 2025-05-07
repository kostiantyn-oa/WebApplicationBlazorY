function initializeTinyMCE(selector) {
    if (typeof tinymce === 'undefined') {
        console.error('TinyMCE not loaded');
        return;
    }

    // Remove existing TinyMCE instance if it exists
    tinymce.remove(selector);

    // Initialize TinyMCE
    tinymce.init({
        selector: selector,
        language: 'uk',
        plugins: 'preview importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media codesample table charmap pagebreak nonbreaking anchor insertdatetime advlist lists wordcount help charmap quickbars emoticons accordion',
        menubar: 'file edit view insert format tools table help',
        toolbar: 'undo redo | accordion accordionremove | blocks fontfamily fontsize | bold italic underline strikethrough | align numlist bullist | link image | table media | lineheight outdent indent | forecolor backcolor removeformat | charmap emoticons | code fullscreen preview | save print | pagebreak anchor codesample | ltr rtl',
        autosave_ask_before_unload: true,
        autosave_interval: '30s',
        autosave_prefix: '{path}{query}-{id}-',
        autosave_restore_when_empty: false,
        autosave_retention: '2m',
        image_advtab: true,
        content_css: '//www.tiny.cloud/css/codepen.min.css',
        importcss_append: true,
        height: 400,
        image_caption: true,
        quickbars_selection_toolbar: 'bold italic | quicklink h2 h3 blockquote quickimage quicktable',
        noneditable_class: 'mceNonEditable',
        toolbar_mode: 'sliding',
        contextmenu: 'link image table',
        setup: function (editor) {
            editor.on('input', function () {
                var textarea = document.querySelector(selector);
                if (textarea) {
                    textarea.value = editor.getContent();
                    textarea.dispatchEvent(new Event('input', { bubbles: true }));
                }
            });
            editor.on('change', function () {
                var textarea = document.querySelector(selector);
                if (textarea) {
                    textarea.value = editor.getContent();
                    textarea.dispatchEvent(new Event('input', { bubbles: true }));
                }
            });
        }
    });
}

function destroyTinyMCE(selector) {
    try {
        if (typeof tinymce !== 'undefined' && tinymce.activeEditor) {
            const editor = tinymce.get(selector.replace('#', ''));
            if (editor) {
                tinymce.remove(selector);
                console.log(`TinyMCE instance removed for ${selector}`);
            } else {
                console.log(`No TinyMCE instance found for ${selector}`);
            }
        } else {
            console.log('TinyMCE not loaded or no active editor');
        }
    } catch (e) {
        console.error(`Error destroying TinyMCE for ${selector}:`, e);
    }
}