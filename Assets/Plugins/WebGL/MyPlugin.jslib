mergeInto(LibraryManager.library, {
    InitKeyboardInterceptor: function() {
        document.addEventListener('keydown', function(event) {
            if (event.ctrlKey && (event.key === 'u' || event.key === 'U')) {
                event.preventDefault();
            }
        }, false);
    },

    JS_CopyToClipboard: function (textPtr) {
        var text = UTF8ToString(textPtr);
        navigator.clipboard.writeText(text).catch(function(err) {
            console.error('Erro ao copiar para o clipboard: ', err);
        });
    }
});