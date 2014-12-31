#ifndef __CENTERTABWIDGET_H
#define __CENTERTABWIDGET_H

#include <QtWidgets/QTabWidget>
#include "AIEditor.h"

namespace Ui
{
	class CenterTabWidget;
}

class CenterTabWidget : public QTabWidget
{
	Q_OBJECT

public:
	explicit CenterTabWidget(QWidget *parent = 0);
	~CenterTabWidget();

private:
	Ui::CenterTabWidget *m_ui;
};

#endif // CENTERTABWIDGET_H