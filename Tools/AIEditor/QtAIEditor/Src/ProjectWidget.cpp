#include "ProjectWidget.h"
#include "ui_ProjectWidget.h"

ProjectWidget::ProjectWidget(QWidget *parent)
	: QDockWidget(parent, 0), m_ui(new Ui::ProjectWidget)
{
	m_ui->setupUi(this);
}

ProjectWidget::~ProjectWidget()
{

}