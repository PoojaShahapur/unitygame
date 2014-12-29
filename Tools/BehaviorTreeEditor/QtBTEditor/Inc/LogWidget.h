#ifndef LOGWIDGET_H
#define LOGWIDGET_H

#include <QtWidgets/QDockWidget>
#include "BTEditor.h"

namespace Ui
{
	class LogWidget;
}

class LogWidget : public QDockWidget
{
	Q_OBJECT

public:
	explicit LogWidget(QWidget *parent = 0);
	~LogWidget();

private:
	Ui::LogWidget *m_ui;
};

#endif // LOGWIDGET_H