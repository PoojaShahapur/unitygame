#ifndef PROJECTWIDGET_H
#define PROJECTWIDGET_H

#include <QtWidgets/QDockWidget>
#include "AIEditor.h"

namespace Ui
{
	class ProjectWidget;
}

class ProjectWidget : public QDockWidget
{
	Q_OBJECT

public:
	explicit ProjectWidget(QWidget *parent = 0);
	~ProjectWidget();

private:
	Ui::ProjectWidget *m_ui;
};

#endif // PROJECTWIDGET_H