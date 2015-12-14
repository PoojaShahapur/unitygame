using System.Collections;

namespace SDK.Lib
{
	public interface fEngineRenderEngine
	{
        void initialize();

        fElementContainer initRenderFor(fRenderableElement element);

        void stopRenderFor(fRenderableElement element);

        void updateCharacterPosition(fCharacter character);

        // KBEN: 
        //void updateEffectPosition(EffectEntity effect);

        void showElement(fRenderableElement element);

        void hideElement(fRenderableElement element);

        void enableElement(fRenderableElement element);

        void disableElement(fRenderableElement element);

        void setCameraPosition(fCamera camera);

        void setViewportSize(float width, float height);

        ArrayList translateStageCoordsToElements(float x, float y);

        void dispose();

        void setSceneLayer(MList<SpriteLayer> value);
	}
}