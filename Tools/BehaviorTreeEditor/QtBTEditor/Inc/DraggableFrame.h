#ifndef DRAGGABLEFRAME_H
#define DRAGGABLEFRAME_H

#include <QtWidgets/QFrame>
#include "BTEditor.h"

namespace Ui
{
	class DraggableFrame;
}

class DraggableFrame : public QFrame
{
	Q_OBJECT

public:
	explicit DraggableFrame(QWidget *parent = 0);
	~DraggableFrame();

private:
	Ui::DraggableFrame *m_ui;
};

#endif // DRAGGABLEFRAME_H