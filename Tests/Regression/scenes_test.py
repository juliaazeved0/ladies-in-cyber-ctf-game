"""Validate serialized connections without importing assets into Unity."""
from pathlib import Path
import re
import unittest

ROOT = Path(__file__).resolve().parents[2]
ASSETS = ROOT / 'Assets'


def documents(path):
    text = path.read_text(errors='replace')
    return {m[2]: (m[1], m[3]) for m in re.finditer(
        r'^--- !u!(\d+) &(-?\d+)[^\n]*\n(.*?)(?=^--- !u!|\Z)', text, re.M | re.S)}


class SceneRegression(unittest.TestCase):
    def test_screen_space_canvases_scale_with_screen_size(self):
        for name in ['PlayerMap.unity', 'BossRoom.unity']:
            docs = documents(ASSETS / 'Scenes' / name)
            canvases = {}
            for kind, body in docs.values():
                go = re.search(r'm_GameObject: \{fileID: (\d+)\}', body)
                if not go:
                    continue
                if kind == '223':
                    mode = re.search(r'm_RenderMode: (\d+)', body)
                    if mode:
                        canvases[go[1]] = mode[1]
            for kind, body in docs.values():
                if kind != '114' or 'm_UiScaleMode:' not in body:
                    continue
                go = re.search(r'm_GameObject: \{fileID: (\d+)\}', body)
                if go and canvases.get(go[1]) == '0':
                    self.assertRegex(body, r'm_UiScaleMode: 1\n')
                    self.assertRegex(body, r'm_ReferenceResolution: \{x: [1-9]\d*, y: [1-9]\d*\}')
                    self.assertRegex(body, r'm_MatchWidthOrHeight: 0\.5')

    def test_outline_shader_has_bounded_webgl_sampling(self):
        shader = (ASSETS / 'Shaders/SpriteOutline.shader').read_text()
        self.assertIn('const int numSamples = 16;', shader)
        self.assertNotIn('_OutlineSampleQuality', shader)

    def test_forensics_panels_registered_and_references_resolve(self):
        docs = documents(ASSETS / 'Scenes/PlayerMap.unity')
        manager = next(body for kind, body in docs.values() if kind == '1001' and
                       'm_SourcePrefab: {fileID: 100100000, guid: 1a144026e8b11c34a9d73930bc3d035e' in body)
        size = int(re.search(r'propertyPath: allPanels.Array.size\n      value: (\d+)', manager)[1])
        slots = {int(m[1]): m[2] for m in re.finditer(
            r'propertyPath: allPanels.Array.data\[(\d+)\]\n      value:[^\n]*\n      objectReference: \{fileID: (\d+)\}', manager)}
        self.assertEqual(set(slots), set(range(size)))
        for ref in slots.values():
            self.assertIn(ref, docs)
        prefab = documents(ASSETS / 'Prefabs/ForenseRoom 1.prefab')
        for source in ['2104220131252094051', '5138657036259939397',
                       '5021793709922560221', '3608001821962150132']:
            self.assertIn(source, prefab)
            self.assertTrue(any(f'm_CorrespondingSourceObject: {{fileID: {source},' in docs[ref][1]
                                for ref in slots.values()), source)

    def test_postit_copies_visible_clue_through_live_manager(self):
        docs = documents(ASSETS / 'Scenes/BossRoom.unity')
        button = docs['1757867406'][1]
        self.assertIn('guid: 4e29b1a8efbd4b44bb3f3716e73f07ff', button)
        self.assertIn('m_Target: {fileID: 101323901}', button)
        self.assertIn('m_MethodName: CopyPostIt', button)
        manager = docs['101323901'][1]
        self.assertIn('m_Enabled: 1', manager)
        self.assertIn('postItText: {fileID: 1757867404}', manager)
        self.assertIn("m_text: 'Q2gzZj", docs['1757867404'][1])
        self.assertIn('terminalPanel: {fileID: 2060862582}', docs['1300983430'][1])

    def test_crypto_lock_restored_in_prefab_used_by_map(self):
        prefab = ASSETS / 'Backup/Julia/Rooms/CriptoRoom 1.prefab'
        lock = documents(prefab)['7946764736940297087'][1]
        self.assertIn('unlockAfterChallenge: CryptoPassword', lock)
        guid = re.search(r'^guid: (\w+)', Path(str(prefab) + '.meta').read_text(), re.M)[1]
        self.assertIn(guid, (ASSETS / 'Scenes/PlayerMap.unity').read_text())

    def test_no_duplicate_scene_file_ids(self):
        for scene in (ASSETS / 'Scenes').glob('*.unity'):
            ids = re.findall(r'^--- !u!\d+ &(-?\d+)', scene.read_text(), re.M)
            self.assertEqual(len(ids), len(set(ids)), scene.name)


if __name__ == '__main__':
    unittest.main()
