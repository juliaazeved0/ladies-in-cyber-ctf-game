const assert = require('node:assert/strict');
const fs = require('node:fs');
const vm = require('node:vm');
const path = require('node:path');
const source = fs.readFileSync(path.join(__dirname, '../../Assets/Plugins/WebGL/MyPlugin.jslib'), 'utf8');

async function checkCopy(mode) {
    const state = { copied: [], appended: 0, removed: 0, focused: 0, selected: 0, listeners: 0 };
    const library = {};
    const document = {
        activeElement: { focus() { state.focused++; } },
        addEventListener() { state.listeners++; },
        createElement() { return { style: {}, setAttribute() {}, select() { state.selected++; } }; },
        body: { appendChild(field) { state.appended++; state.field = field; }, removeChild() { state.removed++; } },
        execCommand(command) { assert.equal(command, 'copy'); state.copied.push(state.field.value); return true; }
    };
    const navigator = mode === 'missing' ? {} : {
        clipboard: { writeText(text) {
            if(mode === 'denied') return Promise.reject(new Error('denied'));
            state.copied.push(text);
            return Promise.resolve();
        } }
    };
    vm.runInNewContext(source, {
        LibraryManager: { library }, mergeInto: Object.assign, UTF8ToString: value => value,
        document, navigator, console
    });
    library.InitKeyboardInterceptor();
    library.InitKeyboardInterceptor();
    assert.equal(state.listeners, 1, 'keyboard listener must not accumulate');
    library.JS_CopyToClipboard('L1C{teste_ç}');
    await Promise.resolve();
    assert.deepEqual(state.copied, ['L1C{teste_ç}']);
    if(mode === 'ok') assert.equal(state.appended, 0);
    else {
        assert.equal(state.appended, 1);
        assert.equal(state.removed, 1);
        assert.equal(state.focused, 1);
        assert.equal(state.selected, 1);
    }
}
(async () => {
    for(const mode of ['ok', 'missing', 'denied']) await checkCopy(mode);
    console.log('Passed 3 WebGL clipboard scenarios (mock browser API).');
})().catch(error => { console.error(error); process.exitCode = 1; });
