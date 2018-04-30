/*
Sprite Sheet Creator
(c) 2016 Digital Ruby, LLC
Created by Jeff Johnson
http://www.digitalruby.com
*/

using UnityEngine;
using UnityEngine.UI;

using System.Collections;

namespace DigitalRuby.SpriteSheetCreator
{

#if UNITY_EDITOR

    public enum SpriteSheetBackgroundColor
    {
        Transparent = 0,
        Black = 1,
        White = 2,
        Magenta = 3,
        Cyan = 4
    }

    [ExecuteInEditMode]
    public class SpriteSheetCreatorScript : MonoBehaviour
    {
        [Header("Sprite Sheet Properties")]
        [Tooltip("Info. Auto-generated from properties.")]
        public string Info;

        [Tooltip("The width of each frame in the spritesheet in pixels")]
        public int FrameWidth = 64;

        [Tooltip("The height of each frame the spritesheet in pixels")]
        public int FrameHeight = 64;

        [Range(2, 64)]
        [Tooltip("The number of rows in the spritesheet")]
        public int Rows = 5;

        [Range(2, 64)]
        [Tooltip("The number of columns in the spritesheet")]
        public int Columns = 5;

        [Tooltip("Background color for the sprite sheet. Use transparent unless your sprite will use additive blend mode, in which case use black. Other colors are available for edge cases.")]
        public SpriteSheetBackgroundColor BackgroundColor = SpriteSheetBackgroundColor.Transparent;
        private readonly Color[] backgroundColors = new Color[] { Color.clear, Color.black, Color.white, new Color(1.0f, 0.0f, 1.0f, 1.0f), Color.cyan };

        [Header("Animation Properties")]
        [Tooltip("The animation to use. If null, AnimationDuration is used and frames are spaced evenly using AnimationDuration / (Rows * Columns).")]
        public Animator Animator;

        [Tooltip("If Animation is null, this is the total seconds to capture animation, with frames being evenly spaced using AnimationDuration / (Rows * Columns).")]
        public float AnimationDuration = 1.0f;

        [Header("Export")]
        [Tooltip("Root object for content that will be exported")]
        public GameObject ExportRoot;

        [Tooltip("The full path and file name to save the saved sprite sheet to. Leave this blank unless you have a specific use case for a different path.")]
        public string SaveFileName;

        [Tooltip("The label to notify that the export is working and then completed")]
        public Text ExportCompleteLabel;

        [Header("Preview")]
        [Tooltip("Particle system for preview once export is done")]
        public ParticleSystem PreviewParticleSystem;

        [Range(1, 60)]
        [Tooltip("Preview FPS")]
        public int PreviewFPS = 24;

        [Header("Scene")]
        [Tooltip("Overlay for aspect ratio")]
        public RectTransform AspectRatioOverlay;

        [Tooltip("Camera to use. Defaults to main camera.")]
        public Camera Camera;

        private bool exporting;
        private RenderTexture renderTexture;

        private Rect CenterSizeInScreen()
        {
            float widthScale = (float)Screen.width / (float)FrameWidth;
            float heightScale = (float)Screen.height / (float)FrameHeight;
            float ratio = Mathf.Min(widthScale, heightScale);
            float newWidth = FrameWidth * ratio;
            float newHeight = FrameHeight * ratio;
            float x = ((float)Screen.width - newWidth) * 0.5f;
            float y = ((float)Screen.height - newHeight) * 0.5f;

            return new Rect(x, y, newWidth, newHeight);
        }

        private void UpdateCamera()
        {
            if (Camera == null)
            {
                Camera = Camera.main;
                if (Camera == null)
                {
                    Camera = Camera.current;
                }
            }
        }

        private void UpdateInfo()
        {
            Info = "Dimensions: " + Width + "x" + Height;
        }

        private void Start()
        {
            UpdateCamera();
            UpdateInfo();
        }

        private void Update()
        {
            UpdateCamera();
            UpdateInfo();
            Rect rect = CenterSizeInScreen();
            AspectRatioOverlay.sizeDelta = new Vector2(rect.width, rect.height);
            if (!Application.isPlaying)
            {
                UpdatePreviewParticleSystem();
            }
        }

        private void ExportFrame(int row, int column)
        {
            float x = ((float)column * (float)FrameWidth) / (float)Width;
            float y = ((float)row * (float)FrameHeight) / (float)Height;
            float w = (float)FrameWidth / (float)Width;
            float h = (float)FrameHeight / (float)Height;

            CameraClearFlags clearFlags = Camera.clearFlags;
            Camera.clearFlags = CameraClearFlags.Depth;
            Camera.targetTexture = renderTexture;
            Rect existingViewportRect = Camera.rect;
            Camera.rect = new Rect(x, 1.0f - y - h, w, h);
            Camera.Render();
            Camera.rect = existingViewportRect;
            Camera.targetTexture = null;
            Camera.clearFlags = clearFlags;
        }

        private void UpdatePreviewParticleSystem()
        {
            if (PreviewParticleSystem == null)
            {
                return;
            }
            PreviewParticleSystem.startLifetime = 100.0f * (float)(Rows * Columns) / (float)PreviewFPS;
            var anim = PreviewParticleSystem.textureSheetAnimation;
            anim.numTilesX = Columns;
            anim.numTilesY = Rows;
            anim.cycleCount = 100;
            anim.enabled = true;
        }

        private void FinishExport()
        {
            RenderTexture.active = renderTexture;
            Texture2D spriteSheetTexture = new Texture2D(Width, Height, TextureFormat.ARGB32, false, false);
            spriteSheetTexture.ReadPixels(new Rect(0, 0, Width, Height), 0, 0);
            RenderTexture.active = null;
            if (string.IsNullOrEmpty(SaveFileName))
            {
                string scenePath = UnityEngine.SceneManagement.SceneManager.GetActiveScene().path;
                scenePath = System.IO.Path.GetDirectoryName(scenePath);
                SaveFileName = System.IO.Path.Combine(scenePath, "SpriteSheet.png");
            }
            byte[] textureBytes = SaveFileName.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase) ? spriteSheetTexture.EncodeToPNG() : spriteSheetTexture.EncodeToJPG(100);
            System.IO.File.WriteAllBytes(SaveFileName, textureBytes);
            exporting = false;
            renderTexture = null;
            UnityEditor.AssetDatabase.Refresh();
            if (PreviewParticleSystem != null)
            {
                UpdatePreviewParticleSystem();
                PreviewParticleSystem.gameObject.SetActive(true);
                PreviewParticleSystem.Stop();
                PreviewParticleSystem.Play();
                ExportRoot.SetActive(false);
            }
            ExportCompleteLabel.text = "Done.";
        }

        private IEnumerator InternalExportCoroutine()
        {
            float frameDelay = 0.0f;
            AnimationClip clip = null;
            if (Animator == null)
            {
                frameDelay = AnimationDuration / (float)(Rows + Columns);
            }
            else
            {
                try
                {
                    clip = Animator.GetCurrentAnimatorClipInfo(0)[0].clip;
                    frameDelay = 1.0f / (float)(Rows * Columns);
                }
                catch
                {
                    Debug.LogError("Unable to get Animator clip. Maybe you should set the Animator to null?");
                    yield break;
                }
            }

            float elapsed = 0.0f;

            for (int row = 0; row < Rows; row++)
            {
                for (int column = 0; column < Columns; column++)
                {
                    if (Animator == null)
                    {
                        yield return new WaitForSeconds(frameDelay);
                    }
                    else
                    {
                        if (Animator.GetCurrentAnimatorStateInfo(0).IsName(clip.name))
                        {
                            Animator.Play(clip.name, -1, elapsed);
                            Animator.speed = 0.0f;
                        }
                        else
                        {
                            Debug.LogError("The animator state must be the same name as the clip.");
                        }
                        yield return new WaitForSeconds(0.001f);
                    }
                    elapsed += frameDelay;
                    ExportFrame(row, column);
                }
            }
            FinishExport();
        }

        public void ExportTapped()
        {
            if (exporting)
            {
                return;
            }

            ExportRoot.SetActive(true);
            PreviewParticleSystem.Stop();
            PreviewParticleSystem.gameObject.SetActive(false);
            exporting = true;
            ExportCompleteLabel.text = "Processing...";
            renderTexture = new RenderTexture(Width, Height, 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
            renderTexture.useMipMap = false;
            renderTexture.filterMode = FilterMode.Point;
            renderTexture.anisoLevel = 0;
            renderTexture.antiAliasing = 1;
            renderTexture.wrapMode = TextureWrapMode.Clamp;
            RenderTexture.active = renderTexture;
            GL.Clear(true, true, backgroundColors[(int)BackgroundColor], 1.0f);
            RenderTexture.active = null;
            StartCoroutine(InternalExportCoroutine());
        }

        public int Width { get { return (FrameWidth * Columns); } }
        public int Height { get { return (FrameHeight * Rows); } }
    }

#endif

}