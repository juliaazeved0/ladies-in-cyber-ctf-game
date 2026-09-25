mergeInto(LibraryManager.library, {
    InitKeyboardInterceptor: function() {
        if (document.__ctfKeyboardInterceptor) return;
        document.__ctfKeyboardInterceptor = true;
        document.addEventListener('keydown', function(event) {
            if (event.ctrlKey && (event.key === 'u' || event.key === 'U')) {
                event.preventDefault();
            }
        }, false);
    },

    JS_CopyToClipboard: function (textPtr) {
        var text = UTF8ToString(textPtr);
        function copyFallback() {
            var previousFocus = document.activeElement;
            var field = document.createElement('textarea');
            field.value = text;
            field.setAttribute('readonly', '');
            field.style.position = 'fixed';
            field.style.opacity = '0';
            document.body.appendChild(field);
            field.select();
            try {
                if (!document.execCommand('copy')) {
                    console.error('O navegador nao permitiu copiar o texto.');
                }
            } catch (error) {
                console.error('Erro ao copiar para o clipboard:', error);
            } finally {
                document.body.removeChild(field);
                if (previousFocus && previousFocus.focus) previousFocus.focus();
            }
        }
        if (navigator.clipboard && navigator.clipboard.writeText) {
            navigator.clipboard.writeText(text).catch(copyFallback);
        } else {
            copyFallback();
        }
    }
});
