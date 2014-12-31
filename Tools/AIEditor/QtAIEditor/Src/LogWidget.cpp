#include "LogWidget.h"
#include "ui_LogWidget.h"

LogWidget::LogWidget(QWidget *parent)
	: QDockWidget(parent, 0), m_ui(new Ui::LogWidget)
{
	m_ui->setupUi(this);
}

LogWidget::~LogWidget()
{

}