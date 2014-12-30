#ifndef DRAGGABLEITEMWIDGET_H
#define DRAGGABLEITEMWIDGET_H

#include <QtWidgets/QWidget>
#include "BTEditor.h"

namespace Ui
{
	class DraggableItemWidget;
}

class DraggableItemWidget : public QWidget
{
	Q_OBJECT

public:
	explicit DraggableItemWidget(QWidget *parent = 0);
	~DraggableItemWidget();

private:
	Ui::DraggableItemWidget *m_ui;
};

#endif // DRAGGABLEITEMWIDGET_H